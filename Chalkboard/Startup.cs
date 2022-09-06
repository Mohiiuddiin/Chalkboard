using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChalkboardAPI.Helpers;
using ChalkboardAPI.IServices;
using ChalkboardAPI.Services;
using ESCHOOL.Common;
using ESCHOOL.IServices;
using ESCHOOL.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//using Microsoft.OpenApi.Models;
using IStudentloginServices = ChalkboardAPI.Services.IVW_StudentLoginServicese;


namespace ESCHOOL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddSingleton<IConfiguration>(Configuration);
            Global.ConnectionsString = Configuration.GetConnectionString("StudentDB");
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));


            //services.AddScoped<IStudentService, StudentService>();
            //services.AddScoped<Ivw_BravoService, vw_BravoServices>();

            services.AddScoped<IClassesService, ClassesService>();
            services.AddScoped<IExamDetailsServices, ExamDetailsServices>();
            services.AddScoped<IExamResultServices, ExamResultServices>();
            services.AddScoped<IExamsServices, ExamsServices>();
            services.AddScoped<IGuardianServices, GuardianServices>();
            services.AddScoped<IPhotosServices, PhotosServices>();
            services.AddScoped<ISectionsServices, SectionsServices>();

            services.AddScoped<ISectionsServices, SectionsServices>();
            services.AddScoped <ISubjectsServices, SubjectsServices> ();
            services.AddScoped <ITaskChatsServices, TaskChatsServices> ();

            services.AddScoped<ITasksServices, TasksServices>();
            services.AddScoped<IStdAttendanceServices, StdAttendanceServices>();
            services.AddScoped<INewsUpdatesServices, NewsUpdatesServices>();
            services.AddScoped<IClassRoutineNewServices, ClassRoutineNewServices>();
            services.AddScoped<IvW_EchoServices, vW_EchoServices>();
            services.AddScoped<Ivw_TasksServices, vw_TasksServices>();

            services.AddScoped<IStudentRegistrationServices, StudentRegistrationServices>();
            services.AddScoped<IStudentprofileServices, StudentprofileServices>();
            services.AddScoped<IGuardiansServices, GuardiansServices>();
            services.AddScoped<IVw_StudentProfileServices, Vw_StudentProfileServices>();
            services.AddScoped<IVw_TeacherNewServices, Vw_TeacherNewServices>();
            services.AddScoped<IVw_TimeTableServices, Vw_TimeTableServices>();
            services.AddScoped<IChatServices, ChatServices>();
            services.AddScoped<ITeachersServices, TeachersServices>();
            services.AddScoped<Ivw_FoxtrotService, vw_FoxtrotService>();
            services.AddScoped<IVw_StudentProfileViewServices, Vw_StudentProfileViewServices>();
            services.AddScoped<IVW_StudentLoginServicese, VW_StudentLoginServicese>();
            services.AddScoped<IStudentsService, StudentsServices>();
            services.AddScoped<Ivw_BravoServices, vw_BravoServices>();
            services.AddScoped<Ivw_AlphaServices, vw_AlphaServices>();




            //services.AddSwaggerGen(swagger =>
            //{
            //    swagger.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Title = "Demo Employee API",
            //        Version = "v1.1",
            //        Description = "API to unerstand request and response schema.",
            //    });
            //});




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseRouting();
            //app.UseAuthentication();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});



            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(x => x.MapControllers());

            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo Employee API");
            //});
        }
    }
}
