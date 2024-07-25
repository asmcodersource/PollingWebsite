# PollingWebsite

PollingWebsite is a web application built with ASP.NET Core\React that allows users to create, view, and respond to simple survey forms. This project aims to provide an easy-to-use interface for managing surveys and collecting responses.

## Features

- **Create Surveys:** Users can create custom survey forms with multiple types of questions.
- **View Surveys:** Users can browse and view the details of available surveys.
- **Respond to Surveys:** Users can submit their responses to surveys and view the results.
- **User Authentication:** Secure login and registration for survey creators and respondents.
- **Responsive Design:** Optimized for use on both desktop and mobile devices.

## Screenshots
![polls](https://github.com/user-attachments/assets/ae770db7-2b58-4bf6-8647-fedbc2bc22c4)
![poll_questions](https://github.com/user-attachments/assets/794aa3bd-4a5a-4644-94ce-8898d006fe21)
![poll_page](https://github.com/user-attachments/assets/021ba84a-ec42-4ae5-966a-a02be8f7a38e)
![poll_answers](https://github.com/user-attachments/assets/ed731836-abe1-4246-a8fc-753453c669a5)
![poll_answer](https://github.com/user-attachments/assets/16c6916d-cfb7-4480-b8b3-d5a9ccc4d094)

  
## Requirements

- .NET 6.0
- MySQL

## Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/your-username/PollingServer.git
    ```

2. Navigate to the project directory:

    ```bash
    cd PollingServer
    ```

3. Restore the necessary dependencies:

    ```bash
    dotnet restore
    ```

4. Configure the database connection string in the `appsettings.json` file.

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=your-server;Database=your-database;User=your-username;Password=your-password;"
      }
    }
    ```

5. Run the application:

    ```bash
    dotnet run
    ```

### Tags

- ASP.NET Core
- API
- Polls
- User Authentication
- MySQL
- .NET 6
- REST API
- C#
