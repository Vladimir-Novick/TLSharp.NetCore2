# TLSharp.NetCore2

Implementing Telegram API in .NET Core2 


_Unofficial_ Telegram (http://telegram.org) client library implemented in C# on .Net Core2. Thanks to Ilya Pirozhenko http://www.sochix.ru/ for TLSharp source code 

# Table of contents

- [How do I add this to my project?](#how-do-i-add-this-to-my-project)
- [Starter Guide](#starter-guide)
  - [Quick configuration](#quick-configuration)
  - [First requests](#first-requests)
  - [Working with files](#working-with-files)
- [License](#license)

# How do I add this to my project?

1. Clone TLSharp.NetCore2 from GitHub
1. Compile source with VS2017 
1. Add reference to ```TLSharp.NetCore2.Core.dll``` to your project.

# Starter Guide

## Quick Configuration
Telegram API isn't that easy to start. You need to do some configuration first.

1. Create a [developer account](https://my.telegram.org/) in Telegram. 
2. Goto [API development tools](https://my.telegram.org/apps) and copy **API_ID** and **API_HASH** from your account. You'll need it later.

## First requests
To start work, create an instance of TelegramClient and establish connection

```csharp 
   var client = new TelegramClient(apiId, apiHash);
   await client.ConnectAsync();
```
Now you can work with Telegram API, but ->
> Only a small portion of the API methods are available to unauthorized users. ([full description](https://core.telegram.org/api/auth)) 

For authentication you need to run following code
```csharp
  var hash = await client.SendCodeRequestAsync("<user_number>");
  var code = "<code_from_telegram>"; // you can change code in debugger

  var user = await client.MakeAuthAsync("<user_number>", hash, code);
``` 

Full code you can see at [AuthUser test](https://github.com/Vladimir-Novick/TLSharp.NetCore2/TLSharp.NetCore2.Tests/TLSharpTests.cs)

When user is authenticated, TLSharp creates special file called _session.dat_. In this file TLSharp store all information needed for user session. So you need to authenticate user every time the _session.dat_ file is corrupted or removed.

You can call any method on authenticated user. For example, let's send message to a friend by his phone number:

```csharp
  //get available contacts
  var result = await client.GetContactsAsync();

  //find recipient in contacts
  var user = result.Users.lists
	  .Where(x => x.GetType() == typeof (TLUser))
	  .Cast<TLUser>()
	  .FirstOrDefault(x => x.phone == "<recipient_phone>");
	
  //send message
  await client.SendMessageAsync(new TLInputPeerUser() {user_id = user.id}, "OUR_MESSAGE");
```

Full code you can see at [SendMessage test](https://github.com/Vladimir-Novick/TLSharp.NetCore2/TLSharp.NetCore2.Tests/TLSharpTests.cs)

To send message to channel you could use the following code:
```csharp
  //get user dialogs
  var dialogs = await client.GetUserDialogsAsync();

  //find channel by title
  var chat = dialogs.chats.lists
    .Where(c => c.GetType() == typeof(TLChannel))
    .Cast<TLChannel>()
    .FirstOrDefault(c => c.title == "<channel_title>");

  //send message
  await client.SendMessageAsync(new TLInputPeerChannel() { channel_id = chat.id, access_hash = chat.access_hash.Value }, "OUR_MESSAGE");
```
Full code you can see at [SendMessageToChannel test](https://github.com/Vladimir-Novick/TLSharp.NetCore2/TLSharp.NetCore2.Tests/TLSharpTests.cs)
## Working with files
Telegram separate files to two categories -> big file and small file. File is Big if its size more than 10 Mb. TLSharp tries to hide this complexity from you, thats why we provide one method to upload files **UploadFile**.

```csharp
	var fileResult = await client.UploadFile("cat.jpg", new StreamReader("data/cat.jpg"));
```

TLSharp provides two wrappers for sending photo and document

```csharp
	await client.SendUploadedPhoto(new TLInputPeerUser() { user_id = user.id }, fileResult, "kitty");
	await client.SendUploadedDocument(
                new TLInputPeerUser() { user_id = user.id },
                fileResult,
                "some zips", //caption
                "application/zip", //mime-type
                new TLVector<TLAbsDocumentAttribute>()); //document attributes, such as file name
```
Full code you can see at [SendPhotoToContactTest](https://github.com/Vladimir-Novick/TLSharp.NetCore2/TLSharp.NetCore2.Tests/TLSharpTests.cs)

To download file you should call **GetFile** method
```csharp
	await client.GetFile(
                new TLInputDocumentFileLocation()
                {
                    access_hash = document.access_hash,
                    id = document.id,
                    version = document.version
                },
                document.size); //size of fileChunk you want to retrieve
```

Full code you can see at [DownloadFileFromContactTest](https://github.com/Vladimir-Novick/TLSharp.NetCore2/TLSharp.NetCore2.Tests/TLSharpTests.cs)

# Available Methods

For your convenience TLSharp have wrappers for several Telegram API methods. You could add your own, see details below.

1. IsPhoneRegisteredAsync
1. SendCodeRequestAsync
1. MakeAuthAsync
1. SignUpAsync
1. GetContactsAsync
1. SendMessageAsync
1. SendTypingAsync
1. GetUserDialogsAsync
1. SendUploadedPhoto
1. SendUploadedDocument
1. GetFile
1. UploadFile
1. SendPingAsync
1. GetHistoryAsync


# License

The MIT License

Copyright (c) 2015 Ilya Pirozhenko http://www.sochix.ru/ ( TLSharp Library )

Copyright (c) 2017 Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/   ( TLSharp.NetCore2 Library )

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

