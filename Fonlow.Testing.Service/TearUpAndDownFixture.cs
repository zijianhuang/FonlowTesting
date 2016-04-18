using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fonlow.Testing
{
    public class TearUpAndDownFixture : IDisposable
    {
        protected TearUpAndDownFixture(Action testClassTearUp, Action testClassTearDown)
        {
            tearUp = testClassTearUp;
            tearDown = testClassTearDown;

            if (tearUp!=null)
            {
                tearUp();
            }
        }
        Action tearUp, tearDown;


        bool disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (tearDown != null)
                    {
                        tearDown();
                    }
                }
                this.disposed = true;
            }
        }


    }

}
