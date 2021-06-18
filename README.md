# Talk Storage Account
This repository contains all of the material for my Storage Account talk

## Dependencies
- [Visual Studio Code](https://code.visualstudio.com/Download)
- [.NET Interactive Notebooks Extension for VS Code](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-interactive-vscode)
- [Visual Studio 2019 with the 'Azure WebJobs Tools' individual component](https://visualstudio.microsoft.com/downloads/)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)

## Presentations
Blob: https://docs.google.com/presentation/d/14FmqcSsq1gTC9FTkSv0rl74dYTPwjJoylWpNREbuIGo/edit?usp=sharing
Table: https://docs.google.com/presentation/d/1lVYnjbq_NlJcSCqHhLGKcYvICdJ5o2aah47Tp3-SWfs/edit?usp=sharing
Queue: https://docs.google.com/presentation/d/1J5FcQNGCrCZ4PCvS1cHlRjoOQKy4VL3E-0EVLiKImNM/edit?usp=sharing


## Notebooks
The `blob-storage-notebook.dib` and `table-storage-notebook.dib` files can be opened in Visual Studio Code with the .NET Interactive Notebooks extensions.

You'll be able to follow along and run code snippet to get started using Blob and Table storage.

## StorageAccount.Playground
The `StorageAccount.Playground` solution is a small prototype of a Producer/Consumer pattern that uses Azure Function, Azure Queue and Azure Table Storage to implement the same design pattern that [Discord uses to store their chat messages](https://blog.discord.com/how-discord-stores-billions-of-messages-7fa6ec7ee4c7).

To run it locally, you'll need to create your own storage account and provide its connection string to the app.

- Create a storage account (`az storage account create`)
- Get the connection string (`az storage account show-connection-string --name <storage-account-name>`)
- In `StorageAccount.BackgroundProcessing`, copy `local.settings.template.json` to `local.settings.json`
- Set `AzureWebJobsStorage` setting value to your storage account's connection string
- In `StorageAccount.Chat`, copy `appsettings.template.json` to `appsettings.json`
- Set `AzureWebJobsStorage` setting value to your storage account's connection string
- Run both programs

