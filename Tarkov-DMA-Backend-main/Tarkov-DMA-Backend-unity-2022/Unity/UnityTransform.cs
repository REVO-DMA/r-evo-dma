using System.Buffers.Binary;
using Tarkov_DMA_Backend.Misc;

namespace Tarkov_DMA_Backend.Unity
{
    public sealed class UnityTransform
	{
		#region Xmm
		private static readonly Vector128<float> Xmm300 = Vector128.Create(-2f, -2f, 2f, 0f);
		private static readonly Vector128<float> Xmm320 = Vector128.Create(-2f, 2f, -2f, 0f);
		private static readonly Vector128<float> Xmm330 = Vector128.Create(2f, -2f, -2f, 0f);
		private static readonly Vector128<float> Xmm610 = Vector128.Create(0f, 0f, 0f, 0f);
		private static readonly Vector128<float> Xmm520 = Vector128.Create(-1f, -1f, -1f, 0f);
		private static readonly Vector128<float> Xmm690 = Vector128.Create(0f, 0f, 0f, 0f);
		private static readonly Vector128<float> XmmDe0 = Vector128.Create(1f, 1f, 1f, 1f);
        #endregion

        private readonly TransformType _transformType;
        private readonly bool _initCached;
        private readonly bool _posCached;

        public UnityTransform(ulong transformInternal, TransformType transformType = TransformType.Normal, bool initCached = false, bool posCached = false)
		{
            _transformType = transformType;
            _initCached = initCached;
            _posCached = posCached;

            HierarchyAddr = Memory.ReadPtr(transformInternal + UnityOffsets.TransformInternal.Hierarchy, _initCached);
            IndicesAddr = Memory.ReadPtr(HierarchyAddr + UnityOffsets.TransformHierarchy.Indices, _initCached);
			VerticesAddr = Memory.ReadPtr(HierarchyAddr + UnityOffsets.TransformHierarchy.Vertices, _initCached);

            if (_transformType == TransformType.PlayerRootPos)
                HierarchyIndex = 1;
            else
                HierarchyIndex = Memory.ReadValue<int>(transformInternal + UnityOffsets.TransformInternal.HierarchyIndex, _initCached);

            Indices = ReadIndices(IndicesAddr, HierarchyIndex + 1, _initCached); // address, count
        }
        public ulong HierarchyAddr { get; }
        public ulong IndicesAddr { get; }
        public ulong VerticesAddr { get; private set; }
		private int[] Indices { get; }
		public int HierarchyIndex { get; private set; }

        #region GetPos
        /// <summary>
        /// Returns the X,Y,Z Position of a Transform.
        /// X,Z = 2d Position
        /// Y = Height
        /// </summary>
        /// <param name="vertices">Vertices/Matrices buffer of transform.</param>
        /// <returns>Vector3</returns>
        public Vector3 GetPosition(Vector128<float>[]? vertices = null)
        {
            if (vertices == null)
                vertices = ReadVertices128(VerticesAddr, 3 * HierarchyIndex + 3, _posCached); // Reference v9/v10 in IDA

            return GetPositionInternal(Indices, vertices, HierarchyIndex, _transformType);
        }

        public static Vector3 GetPositionInternal(int[] indices, Vector128<float>[] vertices, int hierarchyIndex, TransformType transformType = TransformType.Normal)
		{
            var index = indices[hierarchyIndex]; // Indices + 4 * capacity   (was index_relation)
            
            if (transformType == TransformType.PlayerRootPos && index != 0)
                throw new Exception("Invalid index!");

            var result = vertices[3 * hierarchyIndex]; // Vertices + 0x30 * capacity

            int iterations = 0;
            while (index >= 0)
            {
                // perform validations...
                if (transformType == TransformType.PlayerRootPos)
                {
                    if (index > 1) throw new Exception("Invalid index!");
                }
                else
                {
                    if (index > (vertices.Length / 3) - 1) break;
                }
                if (iterations++ >= 1000) throw new Exception("Max SIMD Iterations! Invalid state.");

                // begin iteration...
                var v9 = vertices[3 * index + 1].AsInt32(); // Vertices + 0x30 * index + 0x10

                var v10 = Sse.Multiply(vertices[3 * index + 2], result); // Vertices + 0x30 * index + 0x20

                var v11 = Sse2.Shuffle(v9, 0).AsSingle();
                var v12 = Sse2.Shuffle(v9, 0x71).AsSingle();
                var v13 = Sse2.Shuffle(v9, 0x8E).AsSingle();
                var v14 = Sse2.Shuffle(v9, 0x55).AsSingle();
                var v15 = Sse2.Shuffle(v9, 0xAA).AsSingle();
                var v16 = Sse2.Shuffle(v9, 0xDB).AsSingle();

                result = Sse.Add(
                            Sse.Add(
                                Sse.Add(
                                    Sse.Multiply(
                                        Sse.Subtract(
                                            Sse.Multiply(Sse.Multiply(v11, Xmm330), v13),
                                            Sse.Multiply(Sse.Multiply(v14, Xmm300), v16)),
                                        Sse2.Shuffle(v10.AsInt32(), 0xAA).AsSingle()),
                                    Sse.Multiply(
                                        Sse.Subtract(
                                            Sse.Multiply(Sse.Multiply(v15, Xmm300), v16),
                                            Sse.Multiply(Sse.Multiply(v11, Xmm320), v12)),
                                        Sse2.Shuffle(v10.AsInt32(), 0x55).AsSingle())),
                                Sse.Add(
                                    Sse.Multiply(
                                        Sse.Subtract(
                                            Sse.Multiply(Sse.Multiply(v14, Xmm320), v12),
                                            Sse.Multiply(Sse.Multiply(v15, Xmm330), v13)),
                                        Sse2.Shuffle(v10.AsInt32(), 0).AsSingle()),
                                v10)),
                            vertices[3 * index]); // Vertices + 0x30 * index

                index = indices[index]; // Indices + 4 * index
            }

            // return result
            var pos = result.AsVector3();
            pos.Validate();

            return pos;
        }

        public Quaternion GetRotation(Vector128<float>[]? vertices = null)
        {
            if (vertices == null)
                vertices = ReadVertices128(VerticesAddr, 3 * HierarchyIndex + 3, _posCached); // Reference v9/v10 in IDA
            
            var index = Indices[HierarchyIndex];

            var v6 = vertices[3 * HierarchyIndex].AsInt32();

            if (index >= 0)
            {
                var v9 = Sse2.And(Xmm520.AsInt32(), Xmm610.AsInt32());

                var v11 = Sse.And(Sse2.Shuffle(Vector128<int>.Zero, 0).AsSingle(), Xmm690);

                do
                {
                    var v14 = vertices[3 * index + 1]; // Vertices + 0x30 * index + 0x10

                    var v15 = Sse2.Xor(Sse2.And(Xmm610.AsInt32(), vertices[3 * index + 2].AsInt32()), XmmDe0.AsInt32()); // Vertices + 0x30 * index + 0x20

                    index = Indices[index]; // Indices + 4 * index

                    var v16 = Sse2.Xor(
                        Sse2.And(Xmm610.AsInt32(),
                            Sse.Or(
                                Sse.AndNot(
                                    Xmm690,
                                    Sse.Multiply(Sse2.Shuffle(v15, 0x41).AsSingle(),
                                        Sse2.Shuffle(v15, 0x9A).AsSingle())),
                                v11).AsInt32()),
                        v6).AsSingle();

                    v6 = Sse2.Xor(
                        v9,
                        Sse2.Shuffle(
                            Sse.Subtract(
                                Sse.Subtract(
                                    Sse.Subtract(
                                        Sse.Multiply(v14, Sse2.Shuffle(v16.AsInt32(), 0xD2).AsSingle()),
                                        Sse2.Shuffle(
                                            Sse.Multiply(v14, Sse2.Shuffle(v16.AsInt32(), 0x88).AsSingle()).AsInt32(),
                                            0x1E).AsSingle()),
                                    Sse2.Shuffle(
                                        Sse.Multiply(Sse2.Shuffle(v14.AsInt32(), 0xAF).AsSingle(), v16).AsInt32(),
                                        0x8D).AsSingle()),
                                Sse2.Shuffle(
                                    Sse.Multiply(
                                        Sse.MoveLowToHigh(v14, v14),
                                        Sse2.Shuffle(v16.AsInt32(), 0xF5).AsSingle()).AsInt32(),
                                    0x63).AsSingle()).AsInt32(),
                            0xD2));

                } while (index >= 0);
            }

            var vec4 = v6.AsSingle().AsVector4();

            return new Quaternion
            {
                X = vec4.X,
                Y = vec4.Y,
                Z = vec4.Z,
                W = vec4.W
            };
        }
        #endregion

        #region ReadMem

        /// <summary>
        /// Standalone Indices Read.
        /// </summary>
        /// <returns></returns>
        private static int[] ReadIndices(ulong address, int count, bool useCache)
        {
            if (count <= 0)
                return [];

            const int chunkSize = 4;

            int size = count * chunkSize;
            var buffer = Memory.ReadBuffer(address, size, useCache).AsSpan();

            int[] output = new int[buffer.Length];

            int outputPos = 0;
            for (int i = 0; i < buffer.Length; i += chunkSize)
                output[outputPos++] = BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(i, chunkSize));

            return output;
        }

        /// <summary>
        /// Standalone Vertices Read.
        /// </summary>
        private static Vector128<float>[] ReadVertices128(ulong address, int count, bool useCache)
        {
            if (count <= 0)
                return [];

            const int chunkSize = 16;

            int size = count * chunkSize;
            var buffer = Memory.ReadBuffer(address, size, useCache).AsSpan();
            
            Vector128<float>[] output = new Vector128<float>[count];

            int outputPos = 0;
            for (int i = 0; i < size; i += chunkSize)
            {
                ReadOnlySpan<byte> span = buffer.Slice(i, chunkSize);
                var result = Vector128.Create(span).AsSingle();

                output[outputPos++] = result;
            }

            return output;
        }
        #endregion

        public enum TransformType
        {
            /// <summary>
            /// X,Y,Z Pos for Items, Exfils, etc...
            /// </summary>
            Normal,
            /// <summary>
            /// X,Y,Z for Player Root Pos
            /// </summary>
            PlayerRootPos
        }
    }
}
