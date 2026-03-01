# Supercell Protocol — Authentication & Encryption

## Message Frame

```
[0–1]  Message ID        (uint16, big-endian)
[2–4]  Payload length    (24-bit, big-endian)
[5–6]  Message version   (uint16, big-endian)
[7…]   Payload
```

## Crypto Primitives

| Primitive          | Role                                                         |
|--------------------|--------------------------------------------------------------|
| Curve25519         | ECDH — 32-byte shared secret                                 |
| HChaCha20 ×**17**  | Key derivation — **17 double-rounds** (non-standard)         |
| ChaCha20 ×**8**    | Stream cipher — **8 double-rounds** (non-standard)           |
| Poly1305           | MAC — 16-byte authentication tag                             |
| Blake2b-24         | Nonce derivation — 24-byte hash                              |

> **Box** = Curve25519 ECDH → two HChaCha20 passes → ChaCha20/Poly1305.  
> **SecretBox** = HChaCha20 → ChaCha20/Poly1305 with a pre-shared key.

## Handshake

```
Client                                          Server
  |──── ClientHello (10100) ──────────────────► |  plaintext
  |◄─────────────── ServerHello (20100) ──────── |  plaintext
  |──── LoginMessage (10101) ─────────────────► |  Box
  |◄─── LoginOk (25220) / LoginFailed (20103) ── |  Box
  |──── KeepAlive / game messages ─────────────► |  SecretBox
  |◄──────────────────────── game messages ────── |  SecretBox
```

## Steps

### 1 — ClientHello (plaintext, ID 10100)
Client sends protocol/key/game version, fingerprint SHA1, and device type.

### 2 — ServerHello (plaintext, ID 20100)
Server sends a length-prefixed byte array: the session key. No encryption.

### 3 — Ephemeral keypair + nonces
Client generates 32-byte random private key; derives Curve25519 public key.

### 4 — TempNonce derived; ServerboundNonce generated
`TempNonce = Blake2b-24(ClientPublicKey ‖ ServerPublicKey)`. `ServerboundNonce` = random 24 B, bit 0 cleared.

### 5 — LoginMessage plaintext assembled
Payload: `SessionKey ‖ ServerboundNonce ‖ login fields ‖ 508 zero-bytes`.

### 6 — Box key derivation
`SharedSecret = Curve25519(ClientPrivKey, ServerPubKey)`. Two HChaCha20 rounds (×17 double-rounds each) derive subkey.

### 7 — Box encryption + wire layout (ID 10101)
ChaCha20 (×8 double-rounds) encrypts; Poly1305 tags it. Wire: `ClientPublicKey (32 B) ‖ MAC (16 B) ‖ ciphertext`.

### 8 — Server decrypts LoginMessage, sends response
Server verifies MAC, reads SessionKey + ServerboundNonce, sends LoginOk (25220) or LoginFailed (20103) via Box.

### 9 — Response decryption nonce derived
`DecryptNonce = Blake2b-24(ServerboundNonce ‖ ClientPublicKey ‖ ServerPublicKey)`.

### 10 — SharedKey and ClientboundNonce extracted
BoxOpen output: bytes `0–23` = ClientboundNonce, `24–55` = SharedKey, `56+` = body.

### 11 — Established phase (SecretBox, all further messages)
Nonce increments by 2 per message. Subkey = `HChaCha20(SharedKey, Nonce[:16])`. Wire: `MAC (16 B) ‖ ciphertext`.
