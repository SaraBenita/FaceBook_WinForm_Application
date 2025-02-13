# Facebook Desktop App - Project Overview

## Introduction
This project is a **desktop application** built using **C# .NET WinForms** that integrates with the **Facebook Graph API**. The project was completed as part of two assignments, each focusing on different aspects of software development and design patterns.

---

## Objectives
The project aimed to:
1. **Integrate with the Facebook API** to provide user-friendly interaction with albums, photos, and other user data.
2. **Implement multiple design patterns** (Proxy, Factory Method, Singleton) to improve performance, maintainability, and scalability.
3. **Utilize asynchronous programming and data binding** to enhance UI responsiveness.
4. **Develop and document system architecture** using UML diagrams (Sequence, Class, and Use Case Diagrams).

---

## Technologies Used
- **C# .NET WinForms** – For building the desktop application.
- **Facebook Graph API** – For fetching and managing user data.
- **Object-Oriented Programming (OOP)** – Applied design principles.
- **Multithreading & Asynchronous Programming** – To improve UI performance.
- **Data Binding** – To efficiently link UI components to data sources.
- **UML Diagrams** – For system documentation:
  - **Use Case Diagram** – Describes user interactions with the system.
  - **Sequence Diagram** – Illustrates the flow of data between objects.
  - **Class Diagram** – Shows system architecture and object relationships.

---

## Features Implemented

### 1. GetRandomPhotoFromRandomAlbum
- A game where the user guesses the date a randomly selected photo was taken.
- The system randomly selects an album and a photo.
- The user selects a date, and the system provides feedback.
- Implemented using the **Singleton Pattern** (`GuessTheMomentManager`).

### 2. DownloadAlbum
- Allows the user to select an album, preview its contents, and download it.
- The user selects an album, verifies the images, and chooses a save location.
- Implemented using the **Proxy Pattern** (`UserCashingProxy`) to optimize API calls.

---

## Design Patterns Used
### 1. **Proxy Pattern**
- **Used in:** `UserCashingProxy` for caching Facebook user data.
- **Purpose:** Reduces API calls and improves response time.
- **Implementation:** The proxy class caches user-related data to avoid unnecessary network requests.

### 2. **Factory Method Pattern**
- **Used in:** `FormFactory` to manage UI form creation.
- **Purpose:** Centralizes and standardizes object creation.
- **Implementation:** A static factory method creates forms based on an enum type.

### 3. **Singleton Pattern**
- **Used in:** `GuessTheMomentManager` to ensure a single instance of the game manager.
- **Purpose:** Provides a consistent game state across the application.
- **Implementation:** Uses a private static instance with double-checked locking.

---

## UML Diagrams Created
### 1. **Use Case Diagram**
- Describes the interactions between the user and the system.
- Shows different use cases like "Guess the Photo Date" and "Download Album".

### 2. **Sequence Diagram**
- Illustrates the flow of function calls and object interactions.
- Example: How the system retrieves and displays a Facebook album.

### 3. **Class Diagram**
- Visual representation of the system's architecture.
- Includes relationships between key classes (`UserAdapter`, `FormFactory`, `GuessTheMomentManager`).

---

## Summary
This project demonstrates how **design patterns, asynchronous programming, and API integration** can be effectively combined in a **C# desktop application**. The use of **UML diagrams** provides clear documentation of system architecture and behavior, ensuring maintainability and scalability.
