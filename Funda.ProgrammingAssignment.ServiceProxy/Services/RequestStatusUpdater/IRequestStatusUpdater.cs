namespace Funda.ProgrammingAssignment.ServiceProxy.Services.RequestStatusUpdater
{
    //A simple service used to track the progress of the long-running job
    public interface IRequestStatusUpdater
    {
        void Setup(string text);
        void Initialize(int totalPages, string text = null);
        void Tick();
    }
}