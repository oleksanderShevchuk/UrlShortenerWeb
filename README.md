# UrlShortenerWeb

UrlShortenerWeb is a web application built with ASP.NET Core for shortening URLs. It allows users to generate short URLs from long ones, making them easier to share and manage.

## Features

- **Shorten URLs**: Users can input long URLs and generate short, easy-to-share URLs.
- **Customizable URLs**: Optionally, users can customize their shortened URLs to make them more memorable.
- **View Shortened URLs**: Users can view and manage their list of shortened URLs.
- **Redirect**: When someone accesses a shortened URL, they are redirected to the original long URL.
- **Admin Panel**: Admins have access to additional functionalities such as managing users and URLs.

## Technologies Used

- **ASP.NET Core**: The web application framework used to build the backend.
- **Entity Framework Core**: ORM (Object-Relational Mapping) framework used for interacting with the database.
- **Angular**: Frontend framework for building dynamic web applications.
- **Swagger**: API documentation tool for describing and documenting APIs.
- **JWT (JSON Web Tokens)**: Used for authentication and authorization.
- **SQL Server**: Relational database management system used for storing data.
- **Bootstrap**: Frontend framework for designing responsive and mobile-first websites.
- **Toastr**: JavaScript library for displaying toast notifications.

## Getting Started

1. **Clone the Repository**: Clone this repository to your local machine using `git clone https://github.com/oleksanderShevchuk/UrlShortenerWeb.git`.

2. **Database Configuration**:
    - Open `appsettings.json` and configure your SQL Server connection string.
    - Run Entity Framework migrations to create the database schema: `dotnet ef database update`.

3. **Run the Application**:
    - Navigate to the `UrlShortenerWeb` directory.
    - Run the application using `dotnet run`.

4. **Access the Application**:
    - Once the application is running, open your web browser and navigate to `https://localhost:5001` to access the application.

5. **API Documentation**:
    - Explore the API endpoints and documentation using Swagger UI: `https://localhost:5001/swagger/index.html`.

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvements, please feel free to open an issue or submit a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
