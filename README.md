# 🚗 Fleet Management System (Microservizi) – Specifica Progetto

## 📌 Obiettivo

Progettare e sviluppare un sistema backend basato su **microservizi** per la gestione di una flotta aziendale di auto, utilizzate dagli impiegati per spostamenti di lavoro.

Il sistema deve permettere:

* gestione delle auto aziendali
* gestione degli impiegati
* prenotazione delle auto

---

## 🧱 Architettura

Il sistema è composto da **3 microservizi indipendenti**:

---

### 1. 🚘 Car Service

Gestisce tutte le informazioni relative alle auto.

**Responsabilità:**

* CRUD auto

* Stato auto (semplificato):

  * `AVAILABLE` – disponibile per prenotazione
  * `IN_USE` – attualmente utilizzata
  * `MAINTENANCE` – non disponibile per manutenzione

* Informazioni:

  * targa
  * modello
  * chilometraggio

**Logica manutenzione (semplificata):**

* Il chilometraggio viene aggiornato al termine di ogni viaggio
* Se supera una soglia configurabile, l’auto viene impostata in `MAINTENANCE`
* Una volta completata la manutenzione, lo stato dell’auto passa direttamente da `MAINTENANCE` a `AVAILABLE`
* Nessuno stato intermedio: l’auto è semplicemente utilizzabile o non utilizzabile

---

### 2. 👨‍💼 Employee Service

Gestisce gli impiegati dell'azienda.

**Responsabilità:**

* CRUD impiegati

* Informazioni:

  * nome
  * ruolo
  * email
  * abilitazione alla guida (`abilitazioneGuida: true/false`)

**Regole:**

* solo impiegati con `abilitazioneGuida = true` possono prenotare

---

### 3. 📅 Booking Service

Gestisce le prenotazioni delle auto.

**Responsabilità:**

* Creazione prenotazioni

* Validazione disponibilità auto

* Associazione impiegato ↔ auto

* Stato prenotazione (semplificato):

  * `ACTIVE` – prenotazione attiva (in corso o futura)
  * `COMPLETED` – terminata
  * `CANCELLED` – annullata

---

## 🔄 Flusso Principale

### Creazione Prenotazione

1. L'impiegato invia richiesta al **Booking Service**

2. Il Booking Service:

   * verifica l'impiegato → `abilitazioneGuida = true`
   * verifica disponibilità auto → `stato = AVAILABLE`

3. Se valido:

   * crea la prenotazione
   * aggiorna lo stato dell’auto a `IN_USE` all’inizio del noleggio

---

## 🔌 Comunicazione tra Microservizi

Il sistema utilizza due modalità di comunicazione:

---

### Sincrona (HTTP REST)

Usata per richieste in tempo reale:

* `GET /cars/{id}` – verifica stato auto
* `GET /employees/{id}/eligibility` – verifica abilitazione alla guida

---

### Asincrona (Event-Driven, Kafka)

#### Evento: `TripCompleted`

Pubblicato dal **Booking Service** → consumato dal **Car Service**

**Quando:** il viaggio termina

**Payload:**

```json
{ "vehicleId", "bookingId", "chilometriPercorsi", "timestamp" }
```

**Effetto:**

* aggiornamento chilometraggio
* eventuale passaggio a `MAINTENANCE`

---

## 🗃️ Database (concettuale)

### Car Service

```json
Car {
  id,
  plateNumber,
  model,
  status,
  mileage
}
```

---

### Employee Service

```json
Employee {
  id,
  name,
  role,
  email,
  abilitazioneGuida
}
```

---

### Booking Service

```json
Booking {
  id,
  employeeId,
  carId,
  startTime,
  endTime,
  status
}
```

---

## ⚠️ Problemi da Gestire

### 1. Concorrenza

Due impiegati possono provare a prenotare la stessa auto.

**Soluzione:** controllo lato Booking Service

---

### 2. Consistenza dati

Lo stato dell'auto deve essere coerente con le prenotazioni.

**Strategia:**

* il Car Service è l’unico responsabile delle auto
* il Booking Service non modifica direttamente i dati delle auto

---

### 3. Validazioni

* un impiegato non può prenotare più auto contemporaneamente
* un’auto non può essere prenotata nello stesso intervallo da più utenti
* solo utenti con abilitazione alla guida possono prenotare

---

## 🧪 Endpoint API

### Car Service

* `GET /cars`
* `POST /cars`
* `GET /cars/{id}`
* `PATCH /cars/{id}/status`

---

### Employee Service

* `GET /employees`
* `POST /employees`
* `GET /employees/{id}`
* `GET /employees/{id}/eligibility`

---

### Booking Service

* `POST /bookings`
* `GET /bookings`
* `GET /bookings/{id}`
* `PATCH /bookings/{id}/status`

---

## 🛠️ Stack Tecnologico

* Database: Microsoft SQL Server
* Messaggistica: Kafka
* Container: Docker
* API: REST
* Linguaggio: C# (.NET)
* Documentazione API: Swagger / OpenAPI

---

## 📦 Struttura Progetto per Microservizio

Ogni microservizio deve seguire una struttura modulare e coerente:

```
microservizio.WebApi
microservizio.Business
microservizio.Repository
microservizio.ClientHttp
microservizio.Shared
```

**Descrizione livelli:**

* **WebApi** → esposizione endpoint REST (controller, middleware)
* **Business** → logica applicativa e orchestrazione
* **Repository** → accesso ai dati (DB)
* **ClientHttp** → comunicazione con altri microservizi (HTTP client)
* **Shared** → modelli condivisi, DTO, costanti

---

## 🚀 Implementazione GitHub

Il progetto è gestito tramite GitHub per favorire la collaborazione e il controllo di versione:

* **Repository Management:**
  Gestione del codice sorgente dei microservizi con branch dedicate per feature e bugfix.

* **Collaborazione:**
  Utilizzo di Pull Request per la revisione del codice prima dell'integrazione nel branch principale.

* **Automazione:**
  Configurazione di GitHub Actions per l'integrazione continua (CI), inclusi:

  * build automatica dei microservizi
  * creazione immagini Docker
  * esecuzione test automatici

---

## 🧠 Nota Finale

Il **Booking Service è il cuore del sistema**:

coordina i flussi e mantiene la logica principale.

---

## 📁 Struttura del Progetto

```
📁 FleetManagementSystem/
├── 📁 .github/
│   └── 📁 workflows/
│       ├── 📄 docker-publish.yml
│       └── 📄 nuget-publish.yml
├── 📄 README.md
├── 📄 appunti.txt
├── 📄 comandi_codice.txt
├── 📄 nuget.config
├── 📁 nuget-packages/
│   ├── 📄 CarService.ClientHttp.1.0.0.nupkg
│   ├── 📄 CarService.Shared.1.0.0.nupkg
│   ├── 📄 EmployeeService.ClientHttp.1.0.0.nupkg
│   └── 📄 EmployeeService.Shared.1.0.0.nupkg
└── 📁 microservices/
    ├── 📄 .env
    ├── 📄 docker-compose.yml
    │
    ├── 📁 BookingService/
    │   ├── 📄 .dockerignore
    │   ├── 📄 BookingService.slnx
    │   ├── 📁 BookingService.Business/
    │   │   ├── 📄 BookingService.Business.csproj
    │   │   ├── 📁 Interfaces/
    │   │   │   ├── 📄 IBookingBusinessService.cs
    │   │   │   └── 📄 IBookingEventProducer.cs
    │   │   ├── 📁 Kafka/
    │   │   │   └── 📄 BookingEventProducer.cs
    │   │   └── 📁 Services/
    │   │       └── 📄 BookingBusinessService.cs
    │   ├── 📁 BookingService.ClientHttp/
    │   │   ├── 📄 BookingService.ClientHttp.csproj
    │   │   ├── 📁 Implementations/
    │   │   │   └── 📄 BookingClient.cs
    │   │   └── 📁 Interfaces/
    │   │       └── 📄 IBookingClient.cs
    │   ├── 📁 BookingService.Repository/
    │   │   ├── 📄 BookingService.Repository.csproj
    │   │   ├── 📁 Implementations/
    │   │   │   └── 📄 BookingRepository.cs
    │   │   ├── 📁 Interfaces/
    │   │   │   └── 📄 IBookingRepository.cs
    │   │   ├── 📁 Migrations/
    │   │   │   ├── 📄 20260513072622_InitialCreateBooking.Designer.cs
    │   │   │   ├── 📄 20260513072622_InitialCreateBooking.cs
    │   │   │   └── 📄 BookingDbContextModelSnapshot.cs
    │   │   └── 📁 Persistence/
    │   │       └── 📄 BookingDbContext.cs
    │   ├── 📁 BookingService.Shared/
    │   │   ├── 📄 BookingService.Shared.csproj
    │   │   ├── 📁 DTOs/
    │   │   │   ├── 📄 BookingCreateDto.cs
    │   │   │   └── 📄 BookingDto.cs
    │   │   ├── 📁 Entities/
    │   │   │   └── 📄 Booking.cs
    │   │   ├── 📁 Enums/
    │   │   │   └── 📄 BookingStatus.cs
    │   │   └── 📁 Events/
    │   │       └── 📄 TripCompletedEvent.cs
    │   └── 📁 BookingService.WebApi/
    │       ├── 📄 BookingService.WebApi.csproj
    │       ├── 📄 Dockerfile
    │       ├── 📄 Program.cs
    │       ├── 📄 appsettings.json
    │       ├── 📄 appsettings.Development.json
    │       ├── 📁 Controllers/
    │       │   └── 📄 BookingsController.cs
    │       └── 📁 Properties/
    │           └── 📄 launchSettings.json
    │
    ├── 📁 CarService/
    │   ├── 📄 .dockerignore
    │   ├── 📄 CarService.slnx
    │   ├── 📁 CarService.Business/
    │   │   ├── 📄 CarService.Business.csproj
    │   │   ├── 📁 Interfaces/
    │   │   │   └── 📄 ICarBusinessService.cs
    │   │   ├── 📁 Kafka/
    │   │   │   └── 📄 TripCompletedConsumer.cs
    │   │   └── 📁 Services/
    │   │       └── 📄 CarBusinessService.cs
    │   ├── 📁 CarService.ClientHttp/
    │   │   ├── 📄 CarService.ClientHttp.csproj
    │   │   ├── 📁 Implementations/
    │   │   │   └── 📄 CarClient.cs
    │   │   └── 📁 Interfaces/
    │   │       └── 📄 ICarClient.cs
    │   ├── 📁 CarService.Repository/
    │   │   ├── 📄 CarService.Repository.csproj
    │   │   ├── 📁 Implementations/
    │   │   │   └── 📄 CarRepository.cs
    │   │   ├── 📁 Interfaces/
    │   │   │   └── 📄 ICarRepository.cs
    │   │   ├── 📁 Migrations/
    │   │   │   ├── 📄 20260505202453_InitialCreate.Designer.cs
    │   │   │   ├── 📄 20260505202453_InitialCreate.cs
    │   │   │   └── 📄 CarDbContextModelSnapshot.cs
    │   │   └── 📁 Persistence/
    │   │       └── 📄 CarDbContext.cs
    │   ├── 📁 CarService.Shared/
    │   │   ├── 📄 CarService.Shared.csproj
    │   │   ├── 📁 DTOs/
    │   │   │   ├── 📄 CarDto.cs
    │   │   │   └── 📄 CreateCarDto.cs
    │   │   ├── 📁 Entities/
    │   │   │   └── 📄 Car.cs
    │   │   ├── 📁 Enums/
    │   │   │   └── 📄 CarStatus.cs
    │   │   └── 📁 Events/
    │   │       └── 📄 TripCompletedEvent.cs
    │   └── 📁 CarService.WebApi/
    │       ├── 📄 CarService.WebApi.csproj
    │       ├── 📄 Dockerfile
    │       ├── 📄 Program.cs
    │       ├── 📄 appsettings.json
    │       ├── 📄 appsettings.Development.json
    │       ├── 📁 Controllers/
    │       │   └── 📄 CarsController.cs
    │       └── 📁 Properties/
    │           └── 📄 launchSettings.json
    │
    └── 📁 EmployeeService/
        ├── 📄 .dockerignore
        ├── 📄 EmployeeService.slnx
        ├── 📁 EmployeeService.Business/
        │   ├── 📄 EmployeeService.Business.csproj
        │   ├── 📁 Interfaces/
        │   │   ├── 📄 IEmployeeBusinessService.cs
        │   │   └── 📄 IEmployeeEventProducer.cs
        │   ├── 📁 Kafka/
        │   │   └── 📄 EmployeeEventProducer.cs
        │   └── 📁 Services/
        │       └── 📄 EmployeeBusinessService.cs
        ├── 📁 EmployeeService.ClientHttp/
        │   ├── 📄 EmployeeService.ClientHttp.csproj
        │   ├── 📁 Implementations/
        │   │   └── 📄 EmployeeClient.cs
        │   └── 📁 Interfaces/
        │       └── 📄 IEmployeeClient.cs
        ├── 📁 EmployeeService.Repository/
        │   ├── 📄 EmployeeService.Repository.csproj
        │   ├── 📁 Implementations/
        │   │   └── 📄 EmployeeRepository.cs
        │   ├── 📁 Interfaces/
        │   │   └── 📄 IEmployeeRepository.cs
        │   ├── 📁 Migrations/
        │   │   ├── 📄 20260508120030_InitialCreateEmployee.Designer.cs
        │   │   ├── 📄 20260508120030_InitialCreateEmployee.cs
        │   │   └── 📄 EmployeeDbContextModelSnapshot.cs
        │   └── 📁 Persistence/
        │       └── 📄 EmployeeDbContext.cs
        ├── 📁 EmployeeService.Shared/
        │   ├── 📄 EmployeeService.Shared.csproj
        │   ├── 📁 DTOs/
        │   │   ├── 📄 CreateEmployeeDto.cs
        │   │   └── 📄 EmployeeDto.cs
        │   ├── 📁 Entities/
        │   │   └── 📄 Employee.cs
        │   └── 📁 Events/
        │       └── 📄 EmployeeCreatedEvent.cs
        └── 📁 EmployeeService.WebApi/
            ├── 📄 EmployeeService.WebApi.csproj
            ├── 📄 Dockerfile
            ├── 📄 Program.cs
            ├── 📄 appsettings.json
            ├── 📄 appsettings.Development.json
            ├── 📁 Controllers/
            │   └── 📄 EmployeesController.cs
            └── 📁 Properties/
                └── 📄 launchSettings.json
```