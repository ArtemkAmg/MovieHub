# MovieHub — ASP.NET Core MVC + Email services

## Solution structure

```
MovieHub.sln
├── MovieHubMvc/              ← Web (MVC)
└── MovieHubMvc.Services/     ← Class library (DLL) with IEmailSender
```

## What was implemented (assignment)

1. **Class library** `MovieHubMvc.Services` referenced from the main project.
2. **`IEmailSender`** + **`EmailSender`** (MailKit SMTP).
3. **All config** in `appsettings.json` → section `EmailSettings`.
4. **DI registration** in `Program.cs` as **Scoped** (with comments explaining Transient / Scoped / Singleton).
5. **UI**: Contact page — name, email, optional movie title, message (fits MovieHub theme: suggest a film / feedback).
6. **POST** binds `ContactViewModel` → action calls `_emailSender.SendEmailAsync`.
7. Email goes to **AdminEmail** + confirmation to the user.

## How to send a real email

1. Open `MovieHubMvc/appsettings.json` (and Development) and set:

```json
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": 587,
  "SenderName": "MovieHub",
  "SenderEmail": "your@gmail.com",
  "Username": "your@gmail.com",
  "Password": "xxxx xxxx xxxx xxxx",
  "AdminEmail": "your@gmail.com",
  "UseSsl": true
}
```

2. **Gmail**: enable 2FA → create an **App Password** (Google Account → Security → App passwords). Paste that 16-character password (not your normal Gmail password).

3. Run:

```bash
cd MovieHub
dotnet restore
dotnet run --project MovieHubMvc
```

4. Open **Contact** in the menu → fill the form → Submit → check inbox.

## DI lifetimes (short)

| Lifetime   | When created                         | Use for                          |
|-----------|--------------------------------------|----------------------------------|
| Transient | Every time it is requested           | Lightweight, stateless helpers   |
| Scoped    | Once per HTTP request                | DB / email per form submit       |
| Singleton | Once for the whole app lifetime      | Caches, config wrappers          |

`EmailSender` is registered as **Scoped**.
