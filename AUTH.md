# Supercell Protocol — Authentication & Encryption

## Message Frame

Every TCP message (plaintext or encrypted) uses a fixed 7-byte header.

```
[0–1]  Message ID        (uint16, big-endian)
[2–4]  Payload length    (24-bit, big-endian)
[5–6]  Message version   (uint16, big-endian)
[7…]   Payload
```

## Crypto Primitives

| Primitive   | Role                                           |
|-------------|------------------------------------------------|
| Curve25519  | ECDH — derives a 32-byte shared secret         |
| HChaCha20   | Key derivation — 32-byte key from key + nonce  |
| ChaCha20    | Stream cipher — encrypts the payload           |
| Poly1305    | MAC — 16-byte authentication tag               |
| Blake2b-24  | Nonce derivation — 24-byte hash                |

> **Box** = Curve25519 ECDH + two HChaCha20 rounds + ChaCha20/Poly1305 (NaCl-compatible).  
> **SecretBox** = HChaCha20 + ChaCha20/Poly1305 with a pre-shared symmetric key.

## Handshake Diagram

```
Client                                          Server
  |                                               |
  |──── ClientHello (10100) ──────────────────►  |  plaintext
  |  ◄──────────────────── ServerHello (20100) ──|  plaintext
  |                                               |
  |  [generate ephemeral Curve25519 keypair]      |
  |  [derive TempNonce, ServerboundNonce]          |
  |                                               |
  |──── LoginMessage (10101) ─────────────────►  |  Box-encrypted
  |  ◄────────────── LoginOk (25220) / LoginFailed (20103) ──|  Box-encrypted
  |                                               |
  |  [extract SharedKey + ClientboundNonce]        |
  |                                               |
  |──── KeepAlive / game messages ─────────────► |  SecretBox
  |  ◄──────────────────────── game messages ─── |  SecretBox
```

## Step-by-Step Process

### Step 1 — ClientHello sent (plaintext)
Client sends protocol version, key version, game version, fingerprint SHA1, and device type.

### Step 2 — ServerHello received (plaintext)
Server replies with a length-prefixed raw byte array: the session key. No encryption.

### Step 3 — Ephemeral keypair generated
Client creates 32 random bytes as a private key; derives Curve25519 public key.

### Step 4 — TempNonce derived
`TempNonce = Blake2b-24(ClientPublicKey ‖ ServerPublicKey)`. Server public key is version-specific (see `KEYS.md`).

### Step 5 — ServerboundNonce generated
Client picks a random 24-byte nonce; its lowest bit is cleared to zero.

### Step 6 — LoginMessage plaintext assembled
Payload order: `SessionKey ‖ ServerboundNonce ‖ login fields ‖ 508 zero-bytes padding`.

### Step 7 — ECDH shared secret computed
`SharedSecret = Curve25519(ClientPrivateKey, ServerPublicKey)`. Two HChaCha20 rounds derive a per-message subkey.

### Step 8 — LoginMessage Box-encrypted
ChaCha20 encrypts the payload; Poly1305 produces a 16-byte authentication tag over it.

### Step 9 — LoginMessage wire layout
`ClientPublicKey (32 B) ‖ Poly1305 MAC (16 B) ‖ ChaCha20 ciphertext`. ID 10101.

### Step 10 — Server decrypts LoginMessage
Server derives the same ECDH secret, verifies the MAC, and reads session key and ServerboundNonce.

### Step 11 — Server encrypts response
Server sends LoginOk (25220) or LoginFailed (20103), Box-encrypted using the established ECDH secret.

### Step 12 — Response decryption nonce derived
`DecryptNonce = Blake2b-24(ServerboundNonce ‖ ClientPublicKey ‖ ServerPublicKey)`.

### Step 13 — Server response decrypted
Client calls BoxOpen with the derived nonce and verifies the Poly1305 MAC.

### Step 14 — SharedKey and ClientboundNonce extracted
Plaintext: bytes `0–23` = new ClientboundNonce, bytes `24–55` = SharedKey, bytes `56+` = message body.

### Step 15 — Subsequent client→server messages (SecretBox)
ServerboundNonce increments by 2; payload encrypted with `SecretBox(SharedKey, ServerboundNonce)`.

### Step 16 — Subsequent server→client messages (SecretBox)
ClientboundNonce increments by 2; payload decrypted with `SecretBoxOpen(SharedKey, ClientboundNonce)`.

### Step 17 — SecretBox wire layout
Per-message subkey = `HChaCha20(SharedKey, Nonce[:16])`. Wire: `Poly1305 MAC (16 B) ‖ ciphertext`.
