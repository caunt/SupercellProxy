using System.Buffers.Binary;

namespace SupercellProxy.Keys.Extract;

internal static class MachOKeyVersionExtractor
{
    private const uint Arm64 = 0x0100000C;
    private const uint Call = 0x94000000;
    private const uint CallMask = 0xFC000000;
    private const int MaximumCandidates = 1024;
    private const uint NoOperation = 0xD503201F;
    private const uint Return = 0xD65F03C0;
    private const uint StackStoreMask = 0xFFC003E0;
    private const uint StackStorePairOffset = 0xA90003E0;
    private const uint StackStorePairPreindexed = 0xA98003E0;
    private const uint StackStoreRegisterOffset = 0xF90003E0;

    private enum ValueKind { Object, Field, Address, LoadedInteger }

    public static int? Extract(ReadOnlySpan<byte> binary)
    {
        if (binary.Length < 4)
            return null;

        if (BinaryPrimitives.ReadUInt32BigEndian(binary) == 0xCAFEBABE)
        {
            Require(binary, offset: 0, length: 8);

            uint count = BinaryPrimitives.ReadUInt32BigEndian(binary[4..]);

            if (count is 0 or > 32)
                throw Malformed(message: "Invalid FAT architecture count.");

            Require(binary, offset: 8, count * 20L);

            List<(long Offset, long Length)> ranges = [];
            List<(long Offset, long Length)> selected = [];

            for (int index = 0; index < count; index++)
            {
                // Each FAT entry is 20 big-endian bytes that start with the CPU type the slice is built for.
                ReadOnlySpan<byte> architecture = binary.Slice(8 + (index * 20), length: 20);
                uint cpu = BinaryPrimitives.ReadUInt32BigEndian(architecture);
                uint subtype = BinaryPrimitives.ReadUInt32BigEndian(architecture[4..]) & 0x00FFFFFF;

                // The rest of the entry says where that slice is stored in the file.
                long offset = BinaryPrimitives.ReadUInt32BigEndian(architecture[8..]);
                long length = BinaryPrimitives.ReadUInt32BigEndian(architecture[12..]);

                if (offset < 8 + (count * 20L) || length < 32)
                    throw Malformed(message: "Invalid FAT slice extent.");

                Require(binary, offset, length);

                foreach ((long previous, long size) in ranges)
                {
                    if (offset < previous + size && previous < offset + length)
                        throw Malformed(message: "Overlapping FAT slices.");
                }

                ranges.Add((offset, length));

                if (cpu != Arm64 || subtype != 0)
                    continue;

                selected.Add((offset, length));
            }

            if (selected.Count == 0)
                return null;

            if (selected.Count != 1)
                throw new InvalidOperationException(message: "Multiple supported ARM64 Mach-O slices found.");

            (long Offset, long Length) slice = selected[index: 0];

            binary = binary.Slice(int.CreateTruncating(slice.Offset), int.CreateTruncating(slice.Length));
        }

        if (BinaryPrimitives.ReadUInt32LittleEndian(binary) != 0xFEEDFACF)
            return null;

        Require(binary, offset: 0, length: 32);

        bool supportedArchitecture = BinaryPrimitives.ReadUInt32LittleEndian(binary[4..]) == Arm64
            && (BinaryPrimitives.ReadUInt32LittleEndian(binary[8..]) & 0x00FFFFFF) == 0;

        if (!supportedArchitecture)
            return null;

        uint commandCount = BinaryPrimitives.ReadUInt32LittleEndian(binary[16..]);
        uint commandBytes = BinaryPrimitives.ReadUInt32LittleEndian(binary[20..]);

        if (commandCount > 1024)
            throw Malformed(message: "Mach-O load command count exceeds limit.");

        Require(binary, offset: 32, commandBytes);

        // The load commands are walked from just after the 64-bit header to the end the header declares; an encrypted binary is noted on the way.
        long end = 32L + commandBytes;
        long cursor = 32;
        bool unsupported = false;

        // The walk collects the segments to search, every mapped segment for rebasing, and the chained-fixup tables.
        List<Segment> segments = [];
        List<Segment> allSegments = [];
        List<(long Offset, long Length)> chainedFixups = [];

        for (int index = 0; index < commandCount; index++)
        {
            if (end - cursor < 8)
                throw Malformed(message: "Truncated Mach-O load command.");

            ReadOnlySpan<byte> command = binary[int.CreateTruncating(cursor)..];
            uint kind = BinaryPrimitives.ReadUInt32LittleEndian(command);
            uint size = BinaryPrimitives.ReadUInt32LittleEndian(command[4..]);

            if (size < 8 || size % 4 != 0 || size > end - cursor)
                throw Malformed(message: "Invalid Mach-O load command extent.");

            if (kind == 0x19)
            {
                if (size < 72)
                    throw Malformed(message: "Truncated Mach-O segment.");

                // Where the segment is mapped in memory and how much memory it takes.
                long address = ReadAddress(command[24..]);
                long memory = ReadAddress(command[32..]);

                // Where its bytes are stored in the file, how it may be accessed, and how many section headers follow.
                long offset = ReadAddress(command[40..]);
                long length = ReadAddress(command[48..]);
                uint protection = BinaryPrimitives.ReadUInt32LittleEndian(command[60..]);
                uint sectionCount = BinaryPrimitives.ReadUInt32LittleEndian(command[64..]);

                if (memory > long.MaxValue - address || length > memory || 72L + (sectionCount * 80L) != size)
                    throw Malformed(message: "Invalid Mach-O segment extent.");

                Require(binary, offset, length);
                allSegments.Add(new Segment(address, offset, length, (protection & 4) != 0));

                if (length != 0)
                {
                    foreach (Segment previous in segments)
                    {
                        bool overlaps = (address < previous.Address + previous.Length && previous.Address < address + length)
                            || (offset < previous.Offset + previous.Length && previous.Offset < offset + length);

                        if (overlaps)
                            throw Malformed(message: "Overlapping Mach-O file-backed segments.");
                    }

                    segments.Add(new Segment(address, offset, length, (protection & 4) != 0));
                }
            }
            else if (kind is 0x21 or 0x2C)
            {
                if (size < (kind == 0x2C ? 24 : 20))
                    throw Malformed(message: "Truncated Mach-O encryption command.");

                long offset = BinaryPrimitives.ReadUInt32LittleEndian(command[8..]);
                long length = BinaryPrimitives.ReadUInt32LittleEndian(command[12..]);

                Require(binary, offset, length);

                unsupported |= BinaryPrimitives.ReadUInt32LittleEndian(command[16..]) != 0;
            }
            else if (kind == 0x80000034)
            {
                if (size != 16 || chainedFixups.Count != 0)
                    throw Malformed(message: "Invalid chained-fixups command.");

                long offset = BinaryPrimitives.ReadUInt32LittleEndian(command[8..]);
                long length = BinaryPrimitives.ReadUInt32LittleEndian(command[12..]);

                Require(binary, offset, length);
                chainedFixups.Add((offset, length));
            }

            cursor += size;
        }

        if (cursor != end)
            throw Malformed(message: "Mach-O load command count and size disagree.");

        if (unsupported)
            return null;

        Dictionary<long, long>? pointers = null;

        if (chainedFixups.Count == 1)
        {
            (long Offset, long Length) fixups = chainedFixups[index: 0];

            pointers = ReadChainedPointers(binary, allSegments, fixups.Offset, fixups.Length);

            if (pointers is null)
                return null;
        }

        return Extract(new Image(binary, segments, pointers));
    }

    private static InvalidOperationException Ambiguous()
    {
        return new(message: "Multiple possible Mach-O key-version sources found.");
    }

    private static long Branch(long address, uint word)
    {
        return address + (Signed(word & 0x03FFFFFF, bits: 26) * 4);
    }

    private static int? Extract(Image image)
    {
        long? getter = null;

        foreach (Segment segment in image.Segments)
        {
            if (!segment.Executable)
                continue;

            for (long offset = 0; offset <= segment.Length - 8; offset += 4)
            {
                long address = segment.Address + offset;

                if (image.Word(address) != (0x52800000U | (10100U << 5)) || image.Word(address + 4) != Return)
                    continue;

                if (getter is not null)
                    throw Ambiguous();

                getter = address;
            }
        }

        if (getter is not { } typeGetter)
            return null;

        long? slot = null;

        foreach (Segment segment in image.Segments)
        {
            if (segment.Executable)
                continue;

            for (long offset = 24; offset <= segment.Length - 8; offset += 8)
            {
                long address = segment.Address + offset;

                if (image.Pointer(address) != typeGetter)
                    continue;

                if (slot is not null)
                    throw Ambiguous();

                slot = address;
            }
        }

        if (slot is not { } typeSlot)
            return null;

        if (image.Pointer(typeSlot - 24) is not { } encoder)
            return null;

        if (!image.IsCode(encoder, length: 84))
            return null;

        int? keyField = FindKeyField(image, encoder);

        if (keyField is null)
            return null;

        HashSet<long> setters = [];

        foreach (Segment segment in image.Segments)
        {
            if (!segment.Executable)
                continue;

            for (long address = segment.Address; address <= segment.Address + segment.Length - 8; address += 4)
            {
                uint store = image.Word(address);

                bool fieldStore = (store & 0xFFC00000) == 0xB9000000 && (store & 31) == 1
                    && ((store >> 5) & 31) == 0 && ((store >> 10) & 4095) * 4 == keyField;

                if (fieldStore && image.Word(address + 4) == Return && !setters.Add(address))
                    throw Ambiguous();
            }
        }

        if (setters.Count == 0 || setters.Count > MaximumCandidates)
            return null;

        int? version = null;
        int callers = 0;

        foreach (Segment segment in image.Segments)
        {
            if (!segment.Executable)
                continue;

            for (long offset = 16; offset <= segment.Length - 4; offset += 4)
            {
                long address = segment.Address + offset;
                uint call = image.Word(address);

                if ((call & CallMask) != Call || !setters.Contains(Branch(address, call)))
                    continue;

                if (++callers > MaximumCandidates)
                    throw Ambiguous();

                long? global = FindVersionGlobal(image, segment, address);

                if (global is null || !HasHelloReceiver(image, segment, address, encoder, typeSlot))
                    continue;

                int candidate = BinaryPrimitives.ReadInt32LittleEndian(image.Read(global.Value, length: 4));

                if (candidate <= 0)
                    return null;

                if (version is not null)
                    throw Ambiguous();

                version = candidate;
            }
        }

        return version;
    }

    private static int? FindKeyField(Image image, long encoder)
    {
        RegisterValue?[] registers = new RegisterValue?[32];

        registers[0] = new RegisterValue(ValueKind.Object, Offset: 0);

        List<long> fields = [];
        long? writer = null;
        long? streamOffset = null;
        bool baseCalled = false;

        for (int index = 0; index < 64; index++)
        {
            long address = encoder + (index * 4L);

            if (!image.IsCode(address, length: 4))
                return null;

            uint word = image.Word(address);

            if ((word & CallMask) == Call)
            {
                long target = Branch(address, word);

                if (!image.IsCode(target, length: 4))
                    return null;

                if (!baseCalled && fields.Count == 0 && registers[0] == new RegisterValue(ValueKind.Object, Offset: 0))
                {
                    baseCalled = true;
                }
                else
                {
                    if (!baseCalled || registers[0] is not { Kind: ValueKind.Object, Offset: > 0 } stream)
                        return null;

                    if (registers[1] is not { Kind: ValueKind.Field } field)
                        return null;

                    bool invalidField = (writer is not null && writer != target)
                        || (streamOffset is not null && streamOffset != stream.Offset)
                        || field.Offset < stream.Offset || field.Offset > 65532 || fields.Contains(field.Offset);

                    if (invalidField)
                        return null;

                    writer = target;
                    streamOffset = stream.Offset;

                    fields.Add(field.Offset);

                    if (fields.Count == 5)
                    {
                        for (int fieldIndex = 1; fieldIndex < fields.Count; fieldIndex++)
                        {
                            if (fields[fieldIndex] != fields[index: 0] + (fieldIndex * 4))
                                return null;
                        }

                        return int.CreateTruncating(fields[index: 1]);
                    }
                }

                Array.Clear(registers, index: 0, length: 19);
            }
            else if (MoveOrAdd(word, registers))
            {
            }
            else if ((word & 0xFFC00000) == 0xB9400000)
            {
                RegisterValue? source = registers[(word >> 5) & 31];

                registers[word & 31] = source is { Kind: ValueKind.Object } owner
                    ? new RegisterValue(ValueKind.Field, owner.Offset + (((word >> 10) & 4095) * 4)) : null;
            }
            else if (IsStackStore(word) || word == NoOperation)
            {
                if (baseCalled)
                    return null;
            }
            else
            {
                return null;
            }
        }

        return null;
    }

    private static long? FindVersionGlobal(Image image, Segment segment, long callsite)
    {
        RegisterValue?[] registers = new RegisterValue?[32];
        long start = Math.Max(segment.Address, callsite - 64);

        for (long address = start; address < callsite; address += 4)
        {
            uint word = image.Word(address);

            if ((word & 0x9F000000) == 0x90000000)
            {
                long immediate = ((word >> 29) & 3) | (((word >> 5) & 0x7FFFF) << 2);

                registers[word & 31] = new RegisterValue(ValueKind.Address, (address & ~4095L) + (Signed(immediate, bits: 21) * 4096));
            }
            else if (MoveOrAdd(word, registers))
            {
            }
            else if ((word & 0xFFC00000) == 0xB9400000)
            {
                RegisterValue? source = registers[(word >> 5) & 31];

                registers[word & 31] = source is { Kind: ValueKind.Address } pointer
                    ? new RegisterValue(ValueKind.LoadedInteger, pointer.Offset + (((word >> 10) & 4095) * 4)) : null;
            }
            else if ((word & CallMask) == Call)
            {
                Array.Clear(registers, index: 0, length: 19);
            }
            else if (word != NoOperation)
            {
                Array.Clear(registers);
            }
        }

        return registers[1] is { Kind: ValueKind.LoadedInteger } loaded ? loaded.Offset : null;
    }

    private static bool HasHelloReceiver(Image image, Segment segment, long callsite, long encoder, long typeSlot)
    {
        uint move = image.Word(callsite - 4);

        if ((move & 0xFFE0FFFF) != 0xAA0003E0)
            return false;

        uint receiver = (move >> 16) & 31;

        if (receiver is < 19 or > 28)
            return false;

        long minimum = Math.Max(segment.Address, callsite - 512);

        for (long address = callsite - 8; address >= minimum; address -= 4)
        {
            uint word = image.Word(address);

            if ((word & CallMask) == Call || (word & 0xFFFFFC1F) == 0xD63F0000)
                continue;

            if ((word & 0xFFC00000) == 0xB9000000 || (word & 0xFFC00000) == 0xF9000000)
                continue;

            if ((word & 0x7C000000) == 0x14000000 || (word & 0xFF000010) == 0x54000000)
                return false;

            if ((word & 0x7E000000) is 0x34000000 or 0x36000000)
                return false;

            if ((word & 31) != receiver)
                continue;

            bool savedAllocation = (word & 0xFFE0FFE0) == 0xAA0003E0 && ((word >> 16) & 31) == 0;

            if (!savedAllocation || !image.IsCode(address + 4, length: 4))
                return false;

            uint constructorCall = image.Word(address + 4);

            return (constructorCall & CallMask) == Call && PublishesHello(image, Branch(address + 4, constructorCall), encoder, typeSlot);
        }

        return false;
    }

    private static bool IsStackStore(uint word)
    {
        uint operation = word & StackStoreMask;

        return operation is StackStorePairPreindexed or StackStorePairOffset or StackStoreRegisterOffset;
    }

    private static InvalidDataException Malformed(string message)
    {
        return new(message);
    }

    private static bool MoveOrAdd(uint word, RegisterValue?[] registers)
    {
        int destination = int.CreateTruncating(word & 31);

        if (destination == 31)
            return false;

        if ((word & 0xFFE0FFE0) == 0xAA0003E0)
        {
            registers[destination] = registers[(word >> 16) & 31];

            return true;
        }

        if ((word & 0xFF800000) == 0x91000000)
        {
            RegisterValue? source = registers[(word >> 5) & 31];
            long immediate = ((word >> 10) & 4095) << ((word & 0x400000) != 0 ? 12 : 0);

            registers[destination] = source is { Kind: ValueKind.Object or ValueKind.Address } value
                ? value with { Offset = checked(value.Offset + immediate) } : null;

            return true;
        }

        return false;
    }

    private static bool PublishesHello(Image image, long constructor, long encoder, long typeSlot)
    {
        RegisterValue?[] registers = new RegisterValue?[32];

        for (int index = 0; index < 32; index++)
        {
            long address = constructor + (index * 4L);

            if (!image.IsCode(address, length: 4))
                return false;

            uint word = image.Word(address);

            if ((word & 0x9F000000) == 0x90000000)
            {
                long immediate = ((word >> 29) & 3) | (((word >> 5) & 0x7FFFF) << 2);

                registers[word & 31] = new RegisterValue(ValueKind.Address, (address & ~4095L) + (Signed(immediate, bits: 21) * 4096));
            }
            else if (MoveOrAdd(word, registers))
            {
            }
            else if ((word & 0xFFC00C00) == 0xF8000400 || (word & 0xFFC00000) == 0xF9000000)
            {
                RegisterValue? source = registers[word & 31];

                if (source is not { Kind: ValueKind.Address } pointer)
                    continue;

                long table = pointer.Offset;

                if (table == typeSlot - 40 && image.Pointer(table + 16) == encoder)
                    return true;
            }
            else if ((word & CallMask) == Call)
            {
                Array.Clear(registers, index: 0, length: 19);
            }
            else if (word == Return)
            {
                return false;
            }
            else
            {
                registers[word & 31] = null;
            }
        }

        return false;
    }

    private static long ReadAddress(ReadOnlySpan<byte> bytes)
    {
        ulong value = BinaryPrimitives.ReadUInt64LittleEndian(bytes);

        return value > long.MaxValue ? throw Malformed(message: "Mach-O address exceeds supported range.") : long.CreateTruncating(value);
    }

    private static Dictionary<long, long>? ReadChainedPointers(ReadOnlySpan<byte> binary, List<Segment> segments, long offset, long length)
    {
        ReadOnlySpan<byte> data = binary.Slice(int.CreateTruncating(offset), int.CreateTruncating(length));

        Require(data, offset: 0, length: 28);

        if (BinaryPrimitives.ReadUInt32LittleEndian(data) != 0)
            return null;

        long starts = BinaryPrimitives.ReadUInt32LittleEndian(data[4..]);

        Require(data, starts, length: 4);

        uint count = BinaryPrimitives.ReadUInt32LittleEndian(data[int.CreateTruncating(starts)..]);

        if (count != segments.Count)
            throw Malformed(message: "Chained-fixups segment count differs.");

        Require(data, starts + 4, count * 4L);

        Segment[] bases = [.. segments.Where(static segment => segment.Offset == 0 && segment.Length >= 32)];

        if (bases.Length != 1)
            throw Malformed(message: "Ambiguous Mach-O image base.");

        long imageBase = bases[0].Address;
        Dictionary<long, long> pointers = [];
        HashSet<long> visited = [];

        for (int index = 0; index < count; index++)
        {
            uint relative = BinaryPrimitives.ReadUInt32LittleEndian(data[int.CreateTruncating(starts + 4 + (index * 4L))..]);

            if (relative == 0)
                continue;

            long segmentInformation = starts + relative;

            Require(data, segmentInformation, length: 22);

            // The segment's chained-fixup header gives its own size, the page size and the pointer format.
            ReadOnlySpan<byte> header = data[int.CreateTruncating(segmentInformation)..];
            uint size = BinaryPrimitives.ReadUInt32LittleEndian(header);
            ushort pageSize = BinaryPrimitives.ReadUInt16LittleEndian(header[4..]);
            ushort format = BinaryPrimitives.ReadUInt16LittleEndian(header[6..]);

            // Then where the segment starts and how many of its pages carry fixup chains.
            long segmentOffset = ReadAddress(header[8..]);
            ushort pages = BinaryPrimitives.ReadUInt16LittleEndian(header[20..]);

            if (format is not (2 or 6))
                return null;

            if (size < 22L + (pages * 2L) || pageSize is not (4096 or 16384))
                throw Malformed(message: "Invalid chained-fixups page table.");

            Require(data, segmentInformation, size);

            Segment segment = segments[index];

            bool invalidMapping = segment.Address < imageBase || segmentOffset != segment.Address - imageBase
                || pages > (segment.Length + pageSize - 1) / pageSize;

            if (invalidMapping)
                throw Malformed(message: "Chained-fixups segment mapping differs.");

            for (int page = 0; page < pages; page++)
            {
                ushort first = BinaryPrimitives.ReadUInt16LittleEndian(data[int.CreateTruncating(segmentInformation + 22 + (page * 2L))..]);

                if (first == ushort.MaxValue)
                    continue;

                if ((first & 0x8000) != 0)
                    return null;

                long position = (page * long.CreateChecked(pageSize)) + first;
                long pageEnd = Math.Min((page + 1L) * pageSize, segment.Length);
                bool hasNext = true;

                while (hasNext)
                {
                    if ((position & 3) != 0 || position < page * long.CreateChecked(pageSize) || position > pageEnd - 8)
                        throw Malformed(message: "Chained pointer crosses its file-backed page.");

                    long address = segment.Address + position;

                    if (!visited.Add(address) || visited.Count > 1_000_000)
                        throw Malformed(message: "Duplicate or excessive chained pointers.");

                    ulong raw = BinaryPrimitives.ReadUInt64LittleEndian(binary[int.CreateTruncating(segment.Offset + position)..]);

                    if ((raw >> 63) == 0)
                    {
                        if (((raw >> 44) & 127) != 0)
                            throw Malformed(message: "Reserved chained pointer bits are set.");

                        ulong target = raw & 0xFFFFFFFFFUL;

                        if (format == 6)
                            target = checked(ulong.CreateChecked(imageBase) + target);

                        target |= ((raw >> 36) & 255) << 56;

                        if (target <= long.MaxValue)
                            pointers.Add(address, long.CreateTruncating(target));
                    }

                    long next = long.CreateTruncating((raw >> 51) & 4095) * 4;

                    hasNext = next != 0;

                    if (hasNext && next < 8)
                        throw Malformed(message: "Overlapping chained pointer slots.");

                    position += next;
                }
            }
        }

        return pointers;
    }

    private static void Require(ReadOnlySpan<byte> binary, long offset, long length)
    {
        if (offset < 0 || length < 0 || offset > binary.Length || length > binary.Length - offset)
            throw Malformed(message: "Mach-O file range is out of bounds.");
    }

    private static long Signed(long value, int bits)
    {
        return value >= 1L << (bits - 1) ? value - (1L << bits) : value;
    }

    private readonly ref struct Image
    {
        private readonly ReadOnlySpan<byte> _binary;
        private readonly List<Segment> _segments;
        private readonly Dictionary<long, long>? _pointers;

        public Image(ReadOnlySpan<byte> binary, List<Segment> segments, Dictionary<long, long>? pointers)
        {
            this._binary = binary;
            this._segments = segments;
            this._pointers = pointers;
        }

        public IReadOnlyList<Segment> Segments => _segments;

        public bool IsCode(long address, int length)
        {
            if ((address & 3) != 0)
                return false;

            foreach (Segment segment in _segments)
            {
                if (segment.Executable && address >= segment.Address && address - segment.Address <= segment.Length - length)
                    return true;
            }

            return false;
        }

        public long? Pointer(long address)
        {
            if (_pointers is not null)
                return _pointers.TryGetValue(address, out long target) ? target : null;

            ulong raw = BinaryPrimitives.ReadUInt64LittleEndian(Read(address, length: 8));

            return raw <= long.MaxValue ? long.CreateTruncating(raw) : null;
        }

        public ReadOnlySpan<byte> Read(long address, int length)
        {
            foreach (Segment segment in _segments)
            {
                if (address >= segment.Address && address - segment.Address <= segment.Length - length)
                    return _binary.Slice(int.CreateTruncating(segment.Offset + address - segment.Address), length);
            }

            throw Malformed(message: "Mach-O pointer has no file-backed mapping.");
        }

        public uint Word(long address)
        {
            return BinaryPrimitives.ReadUInt32LittleEndian(Read(address, length: 4));
        }
    }

    private readonly record struct RegisterValue(ValueKind Kind, long Offset);

    private readonly record struct Segment(long Address, long Offset, long Length, bool Executable);
}
