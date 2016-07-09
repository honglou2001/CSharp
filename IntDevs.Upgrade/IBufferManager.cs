using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntDevs.Upgrade
{
    public interface IBufferManager
    {
        byte[] BorrowBuffer();
        void ReturnBuffer(byte[] buffer);
        void ReturnBuffers(IEnumerable<byte[]> buffers);
        void ReturnBuffers(params byte[][] buffers);
    }
}
