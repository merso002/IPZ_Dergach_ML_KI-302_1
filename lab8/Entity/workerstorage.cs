using System.Collections.ObjectModel;

namespace lab8.Entity
{
    public static class WorkerStorage
    {
        public static ObservableCollection<Worker> Workers { get; } =
            new ObservableCollection<Worker>
            {
                new Worker(1, 1, "Іван Іваненко", 160, 100),
                new Worker(2, 2, "Петро Петренко", 170, 120),
                new Worker(3, 3, "Оксана Коваль", 150, 110),
                new Worker(4, 4, "Марія Шевченко", 165, 130)
            };
    }
}

