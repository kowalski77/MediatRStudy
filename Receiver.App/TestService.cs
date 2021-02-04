namespace Receiver.App
{
    public class TestService : ITestService
    {
        public string GetTest()
        {
            return "test";
        }
    }

    public interface ITestService
    {
        string GetTest();
    }
}