using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fonlow.Testing
{
    /// <summary>
    /// A base class for creating fixtures that could setup and tear down. The derived class is to be used with XUnit.ICollectionFixture.
    /// </summary>
    public class SetupAndTearDownFixture : IDisposable
    {
        /// <summary>
        /// Derived class should provide actions of setting up and tearing down resources
        /// </summary>
        /// <param name="testClassSetup">To setup resources</param>
        /// <param name="testClassTearDown"></param>
        protected SetupAndTearDownFixture(Action testClassSetup, Action testClassTearDown)
        {
            tearUp = testClassSetup;
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
