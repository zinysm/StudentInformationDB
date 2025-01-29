using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<StudentDB>()
            .AddScoped<DepartmentRepository>()
            .AddScoped<StudentRepository>()
            .AddScoped<LectureRepository>()
            .AddScoped<DepartmentService>()
            .AddScoped<StudentService>()
            .AddScoped<LectureService>()
            .BuildServiceProvider();

        var menu = new Menu(serviceProvider);
        menu.Show();
    }
}
