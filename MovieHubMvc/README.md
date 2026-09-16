# MovieHubMvc

ASP.NET Core 8 MVC project with custom Bootstrap-inspired MovieHub template.

## How to run

1. Copy `wwwroot/images` folder from the original project ZIP into this project's `wwwroot/images`.
2. Restore client libraries:
   ```
   libman restore
   ```
   (or in Visual Studio: right-click libman.json → Restore Client-Side Libraries)
3. Run:
   ```
   dotnet run
   ```
4. Open https://localhost:5xxx

## What was fixed

- `_Layout.cshtml` now fully uses the template header, CSS (style/header/responsive), Font Awesome + Bootstrap via **libman.json**
- All views (Home, Movies, Favorites, Movie/Details) are clean partials that use the layout
- Controllers added for Movies, Favorites, Movie
- JS navigation updated from *.html to ASP.NET routes (`/Movie/Details?id=...`)
- Active nav highlighting based on current controller

## Structure

- Views/Shared/_Layout.cshtml — main layout with template UI
- libman.json — Bootstrap 5.3.3 + Font Awesome 6.5.1
- wwwroot/css — template styles
- wwwroot/js — client-side movie logic
