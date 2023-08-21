using DinkToPdf.Contracts;
using DinkToPdf;
using CoreLayer.Services.FileManagment;
using CoreLayer.Services.Post;
using CoreLayer.Services.User;
using CoreLayer.Services.UserRole;
using CoreLayer.Services.UserToken;
using BlogWeb.Utilities.PDFCreater;

namespace BlogWeb.Config
{
    public class ConfigServices
    {
        public static void ConfigurationServices(IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IFileManager, FileManager>();
            services.AddTransient<IUserRoleService, UserRoleService>();
            services.AddTransient<IUserTokenService, UserTokenService>();

            services.AddSingleton<IReportService,ReportService>();
            services.AddSingleton(typeof(IConverter),
            new SynchronizedConverter(new PdfTools()));
            services.AddControllers();
        }
    }
}
