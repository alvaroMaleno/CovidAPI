using CoVid.Processes.Interfaces;

namespace CoVid.Processes.Threading
{
    public class TaskableThreadManager
    {
        private static ITaskable[] _oTaskableArray{get;set;}
        private static bool _awaitEachOne;

        public TaskableThreadManager(bool pAwaitEachOne = true, params ITaskable[] pTaskables)
        {
            _awaitEachOne = pAwaitEachOne;
            _oTaskableArray = pTaskables;
        }

        public async void ThreadProc()
        {
            foreach (var oTaskable in _oTaskableArray)
            {
                var oTask = oTaskable.Taskable();
                if(_awaitEachOne && oTask != null && oTask.Exception == null)
                    await oTask;
            }
        }
    }
}