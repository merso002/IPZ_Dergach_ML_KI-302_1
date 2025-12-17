using lab2_8.Entity;
using lab8.Entity;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace lab2_8.api.Get
{
    public static class GetWorkers
    {
        public static Task<ObservableCollection<Worker>> GetWorkersAsync()
        {
            // Повертаємо ОДИН спільний список з WorkerStorage
            return Task.FromResult(WorkerStorage.Workers);
        }
    }
}
