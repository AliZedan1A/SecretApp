# EncriptionApp

EncriptionApp is a simple application for encrypting and decrypting files and folders using secure AES encryption. Built with .NET MAUI and featuring a Blazor-based user interface, the app provides an integrated user experience with support for both Arabic and English languages.

## Features

- **File and Folder Encryption/Decryption**  
  The application encrypts files by appending a `.encrypted` extension or encrypts entire folders by creating a new encrypted directory, and it similarly decrypts them using the same password.

- **Secure AES Encryption**  
  It leverages the `System.Security.Cryptography` library to encrypt data using the AES algorithm. The encryption key is derived from the user’s password via `Rfc2898DeriveBytes`.

- **Asynchronous Processing with Progress Updates**  
  Encryption and decryption operations run in the background using `BackgroundWorker`, with real-time progress updates displayed through a progress bar.

- **File and Folder Selection**  
  Users can select a file or folder from their device using the `CommunityToolkit.Maui.Storage` library.

- **Random Password Generation**  
  The app provides a feature to automatically generate a random password, which is also copied to the clipboard for convenience.

- **Multi-language User Interface**  
  The application supports switching between Arabic and English through a localization service (LocalizationService).

## Project Structure

The project consists of two main components:

### 1. Main Page (User Interface)

The main page includes the following elements:
- **Language Selector:**  
  A toggle allowing users to switch between Arabic and English.
- **Operation Toggle Group:**  
  Buttons to choose between encryption and decryption.
- **File/Folder Selection:**  
  An input field displaying the selected path, along with buttons to pick a file or folder.
- **Password Input Fields:**  
  Fields for entering the password and, when encrypting, confirming it.
- **Random Password Generation:**  
  A button to generate a random password that is automatically copied to the clipboard.
- **Progress Bar and Notifications:**  
  Displays the progress of the operation, status messages, and alerts.

The page also includes comprehensive CSS styling to enhance user experience with animations and visual effects.

### 2. Encryption Service (EncriptionService)

The `EncriptionService` is responsible for the core functionality of the app:
- **File or Folder Encryption:**  
  - When encrypting a single file, it creates a new file with a `.encrypted` extension.
  - When encrypting a folder, it creates a new directory (with a `_enc` suffix) that maintains the original folder structure.
- **File or Folder Decryption:**  
  - For file decryption, the `.encrypted` extension is removed to restore the original file.
  - For folder decryption, the original folder is restored based on the encrypted folder’s name.
- **Background Processing:**  
  The service uses `BackgroundWorker` to perform operations without freezing the user interface, while providing continuous progress updates.
- **Error Handling and Cancellation:**  
  If errors occur (such as an incorrect password or access issues), the operation is halted and the user is notified accordingly.

## How to Use

### Prerequisites

- **.NET 6 or Later**
- **MAUI Workload:**  
  Ensure you have the .NET MAUI development environment set up. For more details, refer to the [MAUI Documentation](https://learn.microsoft.com/en-us/dotnet/maui/overview).
- **Visual Studio 2022** (or later) with MAUI support.

### Getting Started

1. **Clone the Repository from GitHub:**

   ```bash
   git clone https://github.com/AliZedan1A/SecretApp.git
   cd EncriptionApp
