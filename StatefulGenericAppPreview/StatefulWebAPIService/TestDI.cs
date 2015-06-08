

namespace StatefulWebAPIService
{
    public interface ITestDI
    {
        int TestMethod();
    }

    public class TestDI : ITestDI
    {
        public int TestMethod()
        {
            return 7;
        }
    }

}
