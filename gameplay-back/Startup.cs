﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gameplay_back.hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using gameplay_back.Models;

namespace gameplay_back {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_1);
            services.AddCors (o => o.AddPolicy ("CorsPolicy", builder => {
                builder
                    .AllowAnyMethod ()
                    .AllowAnyHeader ()
                    .AllowCredentials ()
                    .AllowAnyOrigin();
            }));
            services.AddSignalR ();

            // Kuldeep | Added For Databasae Creation String
            var connString = Environment.GetEnvironmentVariable("SQLSERVER_HOST") ?? "Server=localhost\\SQLEXPRESS;Database=QuizGamePlayDB;Trusted_Connection=True;";
            services.AddDbContext<efmodel>(options => options.UseSqlServer(connString)); // End Here
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseHsts ();
            }
            app.UseCors ("CorsPolicy");
            app.UseSignalR (routes => {
                routes.MapHub<ChatHub> ("/chathub");
            });
            // app.UseHttpsRedirection (); // To Avoid Redirection On Deployment
            app.UseMvc ();
        }
    }
}