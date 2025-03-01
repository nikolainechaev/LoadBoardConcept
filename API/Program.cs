using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Load Firestore Credentials
var credentialPath = builder.Configuration["Firebase:PrivateKeyPath"];
if (string.IsNullOrEmpty(credentialPath) || !File.Exists(credentialPath))
{
    throw new FileNotFoundException($"Firestore credentials file not found: {credentialPath}");
}

GoogleCredential googleCredential;
using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
{
    googleCredential = GoogleCredential.FromStream(stream);
}

var firestoreClientBuilder = new FirestoreClientBuilder
{
    Credential = googleCredential
};

var firestoreDb = FirestoreDb.Create(builder.Configuration["Firebase:ProjectId"], firestoreClientBuilder.Build());
builder.Services.AddSingleton(firestoreDb);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
