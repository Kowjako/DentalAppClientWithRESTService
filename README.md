# 🧵 Klient do zarządzania przychodniami dentystycznymi
Program przedstawiający prosty program do zarządzania przychodniami dentystycznymi, program zawiera stronę kliencką napisaną na WPF, również dostępne dwie skórki (jasna i ciemna) i ta strona kliencka komunikuje się z REST web serwisem napisanym w ASP.NET Core Web API za pomocą HTTP klienta.

## Część zaimplementowana na REST-serwisie:
- Autentyfikacja za pomocą JWT tokenów  
- Autoryzacja na podstawie ról   
- Filtrowanie, sortowanie, paginacja danych
- Routingi do zarządzania lekarzami, przychodniami, zabiegami
- Walidacja modelu HTTP-zapytań

## Użyte narzędzia  
🔧 **.NET Core 5 / C# 9.0**  
🔧 **Windows Presentation Foundation (WPF)**  
🔧 **ASP.NET Core Web API**  
🔧 **Entity Framework Core**  
🔧 **Fluent Validation**  
🔧 **AutoMapper**  
🔧 **Microsoft SQL Server**  
🔧 **Swagger UI**  
🔧 **NLog**  
🔧 **xUnit / Moq**

## Wygląd aplikacji
![Screenshot_1](https://user-images.githubusercontent.com/19534189/189595846-63f79b33-2052-43d6-8f8f-8aaff284d2c2.jpg)
![Screenshot_7](https://user-images.githubusercontent.com/19534189/189595850-c426bb6c-edd5-442b-9cba-54836ed12fa6.jpg)
![Screenshot_2](https://user-images.githubusercontent.com/19534189/189595858-d6e56467-2e18-4c20-afbb-2e7beb419ca6.jpg)
![Screenshot_3](https://user-images.githubusercontent.com/19534189/189595862-69efe0b9-6775-418c-b88c-657c0bc9e6ca.jpg)
![Screenshot_4](https://user-images.githubusercontent.com/19534189/189595866-f92ede94-e6de-4cb5-a6b6-7080ebce53d1.jpg)
![Screenshot_5](https://user-images.githubusercontent.com/19534189/189595869-c9c8bff4-77da-44a1-9d62-bbbf27d743b7.jpg)


## Dokumentacja API ze Swaggera  
![Screenshot_6](https://user-images.githubusercontent.com/19534189/189595875-d965efc7-a64d-4bac-81bb-9ebe87523d21.jpg)
