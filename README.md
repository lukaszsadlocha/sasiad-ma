# 🏡 Sasiad-Ma  
**"Sąsiad Ma" – An application to strengthen neighborhood bonds**  

This is a community-driven application designed to improve neighborhood relationships by **borrowing and lending useful items for free**.  

The idea is simple:  
- Sometimes you need an item only once or twice a year – like a stroller when going on vacation with your baby. Instead of buying it, you could borrow it from your neighbor.  
- Other times, you want to work in your garden but realize you don’t own a shovel. Instead of purchasing one, you could borrow it from someone in your street or block.  

The goal is to **make borrowing and lending easy, free, and community-focused**, encouraging stronger connections between neighbors.  

---

## ✨ Key Features  
- **Authentication & Login**  
  - Sign up and log in via email with confirmation  
  - Alternative login with Google account  

- **Community Management**  
  - Create and manage your own communities  
  - Join existing communities via an invitation/activation link sent by another member  

- **Item Sharing**  
  - Add items to lend or borrow within your community  
  - Upload images of items for better visibility  
  - Track availability and ownership of items  
  - Search and filter items by name, category, or availability  
  - Create requests for missing items (e.g., "Looking for a ladder")  

- **Communication & Trust**  
  - Community chat / message board for coordination  
  - Reputation system (e.g., number of successful borrow/return actions)  

- **Notifications & Reminders**  
  - Alerts when someone borrows or returns an item  
  - Updates when new items are added  
  - Automatic reminders for return deadlines  

---

## 🏗 Architecture  
The first release will be a **simple monolithic application**:  
- **Back-end**: .NET (Minimal API, Domain-Driven Design principles, Result pattern)  
- **Front-end**: React (designed to be easily portable to mobile)  
- **Database**: PostgreSQL  
- **Hosting**: Initially on Azure or other free cloud hosting solutions  

Planned architecture improvements as the project evolves:  
- Introduce **CQRS (Command Query Responsibility Segregation)** to separate reads/writes for scalability  
- Ensure clean separation of **Core Domain**, **Application**, and **Infrastructure** layers  
- Prefer **Value Objects** and **Aggregates** where it makes sense in the domain  
- Implement **Unit & Integration tests** early (xUnit + SpecFlow for BDD)  
- Future migration towards modular or microservices architecture if community size grows significantly  

---

## 📌 Extra Rules for Prompt  
1. Do not use AutoMapper – map manually for clarity and performance  
2. Do not use MediatR – keep request handling lightweight and explicit  
3. Use Minimal API for simplicity and lightweight endpoints  
4. Apply the **Result pattern** consistently across API, Services, and Domain layers  
5. Avoid paid libraries – stick to open-source solutions  
6. Follow **Domain-Driven Design (DDD)** principles  
7. Keep UI styles minimal – focus on functionality over design  
