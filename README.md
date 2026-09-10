# Market System Demo - Comprehensive Technical Documentation

A modern, modular .NET 10 enterprise application implementing a multi-domain market management system with support for Customers, Sales, Inventory, Finance, Purchases, and Offers. Built with **Clean Architecture**, **Domain-Driven Design (DDD)**, and **Event-Driven Architecture** using Wolverine messaging framework and GraphQL.

---

## Table of Contents

1. [Project Overview](#project-overview)
2. [System Architecture](#system-architecture)
3. [Technology Stack](#technology-stack)
4. [Functional Features](#functional-features)
5. [Non-Functional Features](#non-functional-features)
6. [Project Structure](#project-structure)
7. [Getting Started](#getting-started)
8. [Core Domains](#core-domains)
9. [API Endpoints & Integration](#api-endpoints--integration)
10. [Messaging & Event Handling](#messaging--event-handling)
11. [Database Design](#database-design)
12. [Error Handling & Validation](#error-handling--validation)
13. [Dependency Injection & Modularity](#dependency-injection--modularity)
14. [Configuration & Environment Setup](#configuration--environment-setup)
15. [GraphQL Support](#graphql-support)
16. [Best Practices & Patterns](#best-practices--patterns)

---

## Project Overview

**Market System Demo** is a comprehensive retail/market management solution that demonstrates enterprise-grade architecture patterns in .NET. It manages the complete lifecycle of market operations including:

- **Customer Management**: Registration, payments, debt tracking, and ledger management
- **Sales Operations**: Order creation, item management, refunds, and checkout
- **Inventory Control**: Stock management, product batches, adjustments, and picking strategies
- **Financial Operations**: Shift management, cash movements, expenses, and obligations tracking
- **Purchase Management**: Supplier management, purchase orders, and receipt handling
- **Offer Management**: Dynamic pricing and promotional offer configuration

### Key Characteristics

- **Multi-Domain Architecture**: 6 autonomous business domains with independent data contexts
- **Event-Driven Communication**: Cross-domain integration through async messaging
- **Database per Service**: Each domain maintains its own SQL Server database context
- **Modular Composition**: Plugin-based module system for domain registration and configuration
- **API-First Design**: REST endpoints with Wolverine HTTP handlers + GraphQL support
- **Enterprise Validation**: FluentValidation for command/query validation with detailed error responses

---

## System Architecture

### High-Level Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────┐
│                          MarketApi (Entry Point)                    │
│                    .NET 10 Web API w/ Wolverine                     │
└─────────────────────────────────────────────────────────────────────┘
								   │
		┌──────────────────────────┼──────────────────────────┐
		│                          │                          │
		▼                          ▼                          ▼
┌───────────────────┐  ┌──────────────────────┐  ┌─────────────────────┐
│  HTTP Endpoints   │  │   GraphQL Layer      │  │  Event Bus (CQRS)   │
│  (REST via        │  │  (Hot Chocolate)     │  │  (Wolverine Queue)  │
│   Wolverine)      │  │                      │  │                     │
└────────┬──────────┘  └──────────┬───────────┘  └────────┬────────────┘
		 │                        │                       │
		 └────────────────────────┼───────────────────────┘
								  │
		 ┌────────────────────────┼────────────────────────┐
		 │                        │                        │
	┌────▼─────────┐  ┌──────────▼──────────┐  ┌──────────▼──────────┐
	│   Modules    │  │  Shared Services   │  │  Shared Kernel     │
	│  (IModule)   │  │  (Infrastructure)  │  │  (Exceptions,      │
	└─────────────┘  └────────────────────┘  │   Contracts)       │
		 │                                    └────────────────────┘
		 │
	┌────┴──────────────────────────────────────────┐
	│                                               │
	▼                                               ▼
┌─────────────────┐                    ┌──────────────────────┐
│  6 Autonomous   │                    │  Shared Contracts &  │
│  Business       │                    │  Infrastructure      │
│  Domains        │                    │                      │
└────────────────┘                    └──────────────────────┘
	│
	├─ Customers Domain
	├─ Sales Domain
	├─ Inventory Domain
	├─ Finance Domain
	├─ Purchases Domain
	└─ Offers Domain
```

### Architectural Patterns Applied

#### **1. Clean Architecture (Layered)**
Each domain follows 3-layer architecture:
```
Domain Layer         → Business logic, entities, aggregate roots
	↓
Application Layer   → Commands, queries, event handlers, use cases
	↓
Infrastructure Layer → Data persistence, external integrations
	↓
Presentation Layer  → HTTP endpoints, GraphQL resolvers, DTOs
```

#### **2. Domain-Driven Design (DDD)**
- **Aggregate Roots**: Customer, Order, Product, Shift, PurchaseOrder, Offer
- **Value Objects**: Tracked within aggregates
- **Domain Events**: OrderCompleted, OrderRefunded, PurchaseItemsReceived, etc.
- **Bounded Contexts**: Each domain is a bounded context with independent models

#### **3. CQRS (Command Query Responsibility Segregation)**
- **Commands**: `StartOrderCommand`, `CreateCustomerCommand`, `RecordExpenseCommand`
- **Queries**: Data retrieval operations
- **Event Handlers**: React to domain events asynchronously

#### **4. Event-Driven Architecture**
- **Wolverine Message Bus**: Central event orchestration
- **Transactional Outbox Pattern**: Ensures event persistence with database transactions
- **Cross-Domain Integration**: Domains communicate via events (e.g., OrderCompleted → Finance ledger update)

#### **5. Dependency Injection & Service Locator**
- **IServiceCollection Extension Methods**: `AddCustomersInfrastructure()`, `AddSharedInfrastructure()`
- **IModule Pattern**: Modular registration of domain services
- **Factory Pattern**: `IServiceProvider` with Wolverine for dynamic handler resolution

---

## Technology Stack

### Core Framework & Runtime
- **.NET 10.0**: Latest LTS framework with performance improvements
- **ASP.NET Core 10**: Web API framework with built-in dependency injection
- **Entity Framework Core 10.0.11**: ORM for data access and migrations

### Messaging & Command Handling
- **Wolverine 6.25.1**: Lightweight message bus, command/query dispatcher
  - Runtime compilation for handlers
  - FluentValidation middleware
  - Entity Framework Core transaction support
- **WolverineFx.Http**: REST endpoint handlers with automatic routing
- **WolverineFx.RuntimeCompilation**: Dynamic handler discovery and compilation

### API & GraphQL
- **Hot Chocolate GraphQL**: GraphQL server for queries and mutations
- **Swashbuckle.AspNetCore.SwaggerUI 10.2.3**: Swagger/OpenAPI documentation
- **Microsoft.AspNetCore.OpenApi**: OpenAPI 3.0 support

### Validation
- **FluentValidation**: Declarative validation rules
- **WolverineFx.Http.FluentValidation**: Automatic validation middleware for HTTP endpoints

### Database
- **SQL Server**: Relational database with multi-database support
- **Entity Framework Core**: ORM with migrations, relationships, and complex queries

### Serialization
- **System.Text.Json**: Default JSON serialization with high performance

---

## Functional Features

### ✅ Implemented Capabilities

#### **Customer Management**
- ✓ Create customer accounts with name and phone
- ✓ Track customer payments and payment history
- ✓ Manage customer debt (automatic and manual)
- ✓ Customer ledger for financial reconciliation
- ✓ Customer search and filtering via GraphQL

#### **Sales Operations**
- ✓ Start new orders with customer association
- ✓ Add/remove/update order items with quantity management
- ✓ Checkout and order completion
- ✓ Refund orders with automatic stock restoration
- ✓ Order status tracking and history
- ✓ Automatic inventory deduction on order completion
- ✓ Automatic finance ledger updates on order events

#### **Inventory Management**
- ✓ Product catalog with categories and descriptions
- ✓ Real-time stock tracking per product
- ✓ Stock movements (in/out) with order traceability
- ✓ Product batch management with receipt tracking
- ✓ Inventory adjustments (damages, audits, corrections)
- ✓ Multiple picking strategies (FIFO, LIFO)
- ✓ Stock availability checking before order fulfillment
- ✓ Automatic stock restoration on order refunds

#### **Financial Management**
- ✓ Shift management (open/close with cash tracking)
- ✓ Cash movement recording (deposits, withdrawals)
- ✓ Expense tracking with expense categories
- ✓ Financial obligation management
- ✓ Obligation settlement (partial/full payments)
- ✓ Automatic ledger entries from order events
- ✓ Journal entry creation for audit trails
- ✓ Multi-currency support via Shift context

#### **Purchase Management**
- ✓ Supplier management and maintenance
- ✓ Purchase order creation and tracking
- ✓ Purchase order item management
- ✓ Purchase receipt handling (partial/complete)
- ✓ Automatic inventory updates on purchase receipt
- ✓ Purchase order status workflow

#### **Offer Management**
- ✓ Promotional offer creation and configuration
- ✓ Offer targeting (customer segments, products, time windows)
- ✓ Offer activation/deactivation
- ✓ Multiple offer types support

### 📊 Integration Points

| From Domain | To Domain | Event | Purpose |
|---|---|---|---|
| Sales | Inventory | OrderCompleted | Deduct stock |
| Sales | Inventory | OrderRefunded | Restore stock |
| Sales | Finance | OrderCompleted | Record ledger entry |
| Sales | Customers | OrderCompleted | Track customer purchase |
| Purchases | Inventory | PurchaseReceived | Add stock |
| Purchases | Finance | PurchaseReceived | Record obligation |
| Finance | Customers | ObligationCreated | Track debt |
| Customers | Finance | PaymentReceived | Settle obligations |

---

## Non-Functional Features

### 🔒 Security & Authorization

- **Authentication Support**: Framework ready (can be extended with JWT, OAuth2, Azure AD)
- **Authorization Framework**: Policy-based authorization support
- **HTTPS Enforcement**: Automatic HTTPS redirection in production
- **Secure Configuration**: Connection strings in secure appsettings (environment-based)
- **CORS Support**: Configurable cross-origin requests

### 📈 Performance & Scalability

- **Asynchronous Processing**: All command handlers are async-first
- **Message Persistence**: Wolverine stores messages in SQL Server (durability)
- **Transaction Management**: Entity Framework Core transactions with automatic rollback
- **Connection Pooling**: SQL Server connection reuse through EF Core
- **Minimal Allocations**: System.Text.Json for zero-copy serialization
- **Static Web Assets Compression**: Built-in middleware for compression

### 🛡️ Reliability & Fault Tolerance

- **Global Exception Handler**: Centralized error handling with problem details
- **Command Validation**: All commands validated before execution
- **Transactional Consistency**: Database operations wrapped in transactions
- **Event Durability**: Transactional outbox pattern ensures no event loss
- **Graceful Degradation**: Failed validations return 400 with detailed error messages
- **Health Checks Ready**: Framework support for custom health checks

### 📊 Observability & Monitoring

- **Structured Logging**: ApplicationInsights-ready logging infrastructure
- **OpenAPI/Swagger**: Automatic API documentation generation
- **GraphQL Introspection**: Full GraphQL schema documentation
- **Event Tracing**: Wolverine message tracking and auditing
- **SQL Server Diagnostics**: Connection timeout and command timeout configuration

### 🏗️ Maintainability & Code Quality

- **Separation of Concerns**: Clear layering (Domain, Application, Infrastructure, Presentation)
- **Dependency Injection**: Loose coupling through abstractions
- **Modular Design**: IModule pattern for independent domain composition
- **Consistent Naming**: Clear naming conventions across all layers
- **Reusable Patterns**: Template patterns for commands, handlers, and validators
- **No Code Duplication**: Shared infrastructure and contracts

---

## Project Structure

### Directory Organization

```
MarketSolution/
├── MarketApi/                           # Entry point - Web API host
│   ├── Program.cs                       # Startup configuration
│   ├── appsettings.json                # Configuration
│   └── MarketApi.csproj
│
├── SharedKernel/                        # Cross-cutting domain concepts
│   ├── Exceptions/                      # CommandValidationException, etc.
│   └── SharedKernel.csproj
│
├── SharedContracts/                     # Contracts, interfaces, enums
│   ├── Inventory/Interfaces/            # IInventoryService
│   ├── Inventory/Commands/              # RestoreStockCommand, DeductStockCommand
│   └── SharedContracts.csproj
│
├── SharedInfrastructure/                # Shared services and infrastructure
│   ├── DependancyInjections/           # AddSharedInfrastructure()
│   ├── ExeptionHandling/               # GlobalExceptionHandler
│   ├── Validations/                    # Validation failure handlers
│   └── SharedInfrastructure.csproj
│
├── SharedPresentation/                  # Shared presentation components
│   ├── Common/                         # IModule, ModuleExtensions
│   ├── GraphQL/                        # GraphQLSharedExtensions
│   └── SharedPresentation.csproj
│
├── [DOMAIN]/Domain/                     # Domain logic (6 domains)
├── [DOMAIN]/Application/                # Application services (6 domains)
├── [DOMAIN]/Infrastructure/             # Data access & persistence (6 domains)
├── [DOMAIN]/Presentation/               # HTTP/GraphQL handlers (6 domains)
│
├── Customers/
│   ├── Customers.Domain/
│   │   └── Entities/                   # Customer, CustomerLedger
│   ├── Customers.Application/
│   │   ├── Features/                   # Payments, Debts, Customers
│   │   ├── EventHandlers/              # OrderCompleted, OrderRefunded
│   │   └── Abstractions/               # ICustomersDataContext
│   ├── Customers.Infrastructure/
│   │   ├── Data/                       # CustomersDbContext
│   │   └── DependancyInjections/
│   └── Customers.Presentation/
│       ├── DependancyInjections/       # CustomersModule, GraphqlDependancyInjections
│       └── Endpoints/                  # HTTP handlers
│
├── Sales/                               # Similar structure to Customers
│   ├── Sales.Domain/
│   │   └── Entities/                   # Order, OrderItem, CheckoutResult
│   ├── Sales.Application/
│   │   ├── Features/                   # StartOrder, AddOrderItem, Refund, DeleteItem, UpdateQuantity, Payments
│   │   └── Abstractions/
│   ├── Sales.Infrastructure/
│   │   └── Data/
│   └── Sales.Presentation/
│
├── Inventory/                           # Similar structure (6 layers)
│   ├── Inventory.Domain/
│   │   └── Entities/                   # Product, Category, StockMovement, ProductBatch, Adjustment, InventoryItem
│   ├── Inventory.Application/
│   │   ├── Features/
│   │   │   ├── Products/
│   │   │   ├── Stocks/
│   │   │   └── ProductBatches/
│   │   └── Abstractions/
│   ├── Inventory.Infrastructure/
│   └── Inventory.Presentation/
│
├── Finance/                             # Similar structure (6 layers)
│   ├── Finance.Domain/
│   │   └── Entities/                   # Shift, CashMovement, Expense, FinancialObligation, JournalEntry
│   ├── Finance.Application/
│   │   ├── Features/
│   │   │   ├── Shifts/
│   │   │   ├── Expenses/
│   │   │   ├── CashMovements/
│   │   │   ├── Obligations/
│   │   │   └── Ledger/
│   │   └── Abstractions/
│   ├── Finance.Infrastructure/
│   └── Finance.Presentation/
│
├── Purchases/                           # Similar structure (6 layers)
│   ├── Purchases.Domain/
│   │   └── Entities/                   # Supplier, PurchaseOrder, PurchaseReceipt
│   ├── Purchases.Application/
│   ├── Purchases.Infrastructure/
│   └── Purchases.Presentation/
│
├── Offers/                              # Similar structure (6 layers)
│   ├── Offers.Domain/
│   │   └── Entities/                   # Offer, OfferTarget
│   ├── Offers.Application/
│   ├── Offers.Infrastructure/
│   └── Offers.Presentation/
│
└── MarketSolution.slnx                 # Solution file
```

### Layer Responsibilities

#### **Domain Layer** (`[Domain].Domain.csproj`)
- **Purpose**: Pure business logic with no external dependencies
- **Contents**:
  - Aggregate roots (Customer, Order, Product, etc.)
  - Value objects
  - Domain events
  - Business rules and invariants
- **Dependencies**: Only SharedKernel
- **Example**: `Customers.Domain/Entities/Customer.cs` - defines customer properties and business rules

#### **Application Layer** (`[Domain].Application.csproj`)
- **Purpose**: Use case orchestration and cross-domain coordination
- **Contents**:
  - Command handlers (modifying operations)
  - Query handlers (read operations)
  - Event handlers (responding to domain/integration events)
  - DTOs and mappers
  - Application abstractions (IDataContext)
- **Dependencies**: Domain layer + SharedContracts
- **Example**: `Customers.Application/Features/Payments/Commands/AddCustomerPayment/` - handles payment recording

#### **Infrastructure Layer** (`[Domain].Infrastructure.csproj`)
- **Purpose**: Data persistence and external integrations
- **Contents**:
  - DbContext (Entity Framework)
  - Repository implementations (if used)
  - Database migrations
  - External service clients
  - Dependency injection configuration
- **Dependencies**: Application + Domain layers
- **Example**: `Customers.Infrastructure/Data/CustomersDbContext.cs` - database configuration

#### **Presentation Layer** (`[Domain].Presentation.csproj`)
- **Purpose**: HTTP/GraphQL API contracts and handlers
- **Contents**:
  - HTTP endpoints (Wolverine handlers)
  - GraphQL types and resolvers
  - Request/response DTOs
  - Module registration (IModule)
  - Dependency injection setup
- **Dependencies**: Application layer
- **Example**: `Customers.Presentation/Endpoints/CustomerEndpoints.cs` - HTTP handlers

#### **Shared Layer** (`Shared*.csproj`)
- **Purpose**: Cross-cutting concerns
- **Contents**:
  - Common exceptions (SharedKernel)
  - Shared interfaces (SharedContracts)
  - Infrastructure utilities (SharedInfrastructure)
  - Presentation conventions (SharedPresentation)

---

## Getting Started

### Prerequisites

- **.NET 10 SDK** or later
- **SQL Server 2019** or later (or SQL Server Express LocalDB)
- **Visual Studio 2022** or later (or any .NET-compatible IDE)
- **Git** for repository cloning

### Installation & Setup

#### 1. **Clone Repository**
```powershell
git clone https://github.com/CodexaorgRMS/RMS.git
cd MarketsystemDemo
```

#### 2. **Restore Dependencies**
```powershell
dotnet restore MarketSolution.slnx
```

#### 3. **Configure Database Connection**

Edit `MarketApi\appsettings.json`:
```json
{
  "ConnectionStrings": {
	"Constr": "Server=.;Database=SuperMarketDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;Command Timeout=300;"
  }
}
```

**Connection String Options**:
- **LocalDB**: `(localdb)\mssqllocaldb`
- **SQL Server Express**: `.\SQLEXPRESS` or `localhost`
- **Azure SQL**: `Server=your-server.database.windows.net;Database=...;User Id=...;Password=...;`

#### 4. **Create Database & Run Migrations**

```powershell
# Build the solution
dotnet build MarketSolution.slnx

# Run database migrations (auto-create on startup)
# The application will create tables on first run if AutoBuildMessageStorageOnStartup is enabled
```

#### 5. **Run Application**

```powershell
# From MarketApi directory
dotnet run --project MarketApi/MarketApi.csproj

# Or via Visual Studio: F5 / Debug Start
```

**Default Endpoints**:
- **API Documentation**: `https://localhost:5001/swagger`
- **OpenAPI Spec**: `https://localhost:5001/openapi/v1.json`
- **GraphQL Endpoint**: `https://localhost:5001/graphql`

### Project Setup for Development

#### **Visual Studio Solution Structure**
1. Open `MarketSolution.slnx` in Visual Studio 2022
2. Right-click Solution → Properties → Set MarketApi as startup project
3. Build Solution (Ctrl+Shift+B)
4. Run (F5) - Application starts on HTTPS port 5001

#### **Command Line Development**
```powershell
# Restore and build
dotnet restore
dotnet build

# Run with watch mode for development
dotnet watch --project MarketApi/MarketApi.csproj run

# Run tests (if test projects exist)
dotnet test
```

---

## Core Domains

### 1. **Customers Domain**

#### Purpose
Manage customer accounts, track payments, and maintain financial ledgers for customers.

#### Key Entities
- **Customer** (Aggregate Root)
  - `CustomerId`, `Name`, `Phone`
  - `TotalDebt`, `TotalPaid`
  - Business rule: Cannot exceed debt limit

- **CustomerLedger** (Detail entity)
  - Tracks all financial transactions
  - Linked to orders and payments

#### Key Operations
```csharp
// Create customer
var cmd = new CreateCustomerCommand("John Doe", "+1-555-0100");

// Record payment
var cmd = new AddCustomerPaymentCommand(customerId, "ORD-001", 150.00m);

// Track debt (automatic from orders or manual)
var cmd = new AddManualCustomerDebtCommand(customerId, 100.00m, reason);
```

#### Event Integration
- **Listens to**: OrderCompleted, OrderRefunded (from Sales)
- **Publishes**: CustomerPaymentReceived
- **Effect**: Updates customer ledger automatically

#### Database Schema
```sql
-- Customers Domain Database
CREATE TABLE Customers (
  CustomerId UNIQUEIDENTIFIER PRIMARY KEY,
  Name NVARCHAR(255),
  Phone NVARCHAR(20),
  TotalDebt DECIMAL(18,2),
  TotalPaid DECIMAL(18,2),
  CreatedAt DATETIME
);

CREATE TABLE CustomerLedgers (
  LedgerId UNIQUEIDENTIFIER PRIMARY KEY,
  CustomerId UNIQUEIDENTIFIER FOREIGN KEY,
  Type NVARCHAR(50), -- 'Payment', 'Debt', 'Adjustment'
  Amount DECIMAL(18,2),
  ReferenceNumber NVARCHAR(100),
  TransactionDate DATETIME
);
```

---

### 2. **Sales Domain**

#### Purpose
Manage order lifecycle: creation, item management, checkout, fulfillment, and refunds.

#### Key Entities
- **Order** (Aggregate Root)
  - `OrderId`, `OrderNumber`, `CustomerId`
  - `Status` (Pending, Confirmed, Completed, Refunded)
  - `TotalAmount`, `OrderDate`

- **OrderItem** (Detail entity)
  - `OrderItemId`, `OrderId`, `ProductId`
  - `Quantity`, `UnitPrice`, `Total`

- **CheckoutResult** (Value object)
  - Contains order result details and timestamps

#### Key Operations
```csharp
// Start order
var cmd = new StartOrderCommand(customerId);

// Add item to order
var cmd = new AddOrderItemCommand(orderNumber, productId, quantity: 5);

// Update quantity
var cmd = new UpdateQuantityItemCommand(orderItemId, quantity: 3, orderNumber);

// Remove item
var cmd = new DeleteOrderItemCommand(orderNumber, orderItemId);

// Refund entire order
var cmd = new RefundOrderCommand(orderId);
```

#### Event Integration
- **Publishes**: 
  - `OrderCompleted` → triggers inventory deduction, finance ledger creation
  - `OrderRefunded` → triggers inventory restoration, customer debt reversal
- **Listens to**: CustomerPaymentReceived (for payment tracking)

#### Database Schema
```sql
-- Sales Domain Database
CREATE TABLE Orders (
  OrderId UNIQUEIDENTIFIER PRIMARY KEY,
  OrderNumber NVARCHAR(100) UNIQUE,
  CustomerId UNIQUEIDENTIFIER,
  Status NVARCHAR(50),
  TotalAmount DECIMAL(18,2),
  OrderDate DATETIME,
  CompletedDate DATETIME NULL
);

CREATE TABLE OrderItems (
  OrderItemId UNIQUEIDENTIFIER PRIMARY KEY,
  OrderId UNIQUEIDENTIFIER FOREIGN KEY,
  ProductId UNIQUEIDENTIFIER,
  Quantity INT,
  UnitPrice DECIMAL(18,2),
  Total DECIMAL(18,2)
);
```

---

### 3. **Inventory Domain**

#### Purpose
Maintain product catalog, track stock levels, manage batches, and handle picking strategies.

#### Key Entities
- **Product** (Aggregate Root)
  - `ProductId`, `Name`, `Description`, `CategoryId`
  - `CurrentStock`, `ReorderLevel`
  - `PickingStrategy` (FIFO, LIFO)

- **Category** (Aggregate Root)
  - Organizes products

- **InventoryItem** (Detail entity)
  - Per-product stock tracking

- **StockMovement** (Audit trail)
  - Tracks all in/out movements
  - Links to orders/purchases

- **ProductBatch** (Lot tracking)
  - Serial batch tracking with expiry dates
  - Quality assurance support

- **Adjustment** (Correction)
  - Inventory adjustments (damages, shrinkage, audits)

#### Key Operations
```csharp
// Check stock availability
var cmd = new CheckStockAvailabilityCommand(productId, requiredQty);

// Deduct stock (on order completion)
var cmd = new DeductStockCommand(productId, quantity, orderId);

// Restore stock (on order refund)
var cmd = new RestoreStockCommand(productId, quantity, orderId);

// Update product
var cmd = new UpdateProductCommand(productId, "New Name", description, categoryId);

// Change picking strategy
var cmd = new UpdateProductPickingStrategyCommand(productId, PickingStrategy.LIFO);
```

#### Event Integration
- **Listens to**:
  - `OrderCompleted` → deduct stock automatically
  - `OrderRefunded` → restore stock automatically
  - `PurchaseItemsReceived` → add stock from purchase receipt
- **Publishes**: Stock level warnings, batch expiry alerts (extensible)

#### Database Schema
```sql
-- Inventory Domain Database
CREATE TABLE Products (
  ProductId UNIQUEIDENTIFIER PRIMARY KEY,
  Name NVARCHAR(255),
  Description NVARCHAR(MAX),
  CategoryId UNIQUEIDENTIFIER,
  CurrentStock INT,
  ReorderLevel INT,
  PickingStrategy NVARCHAR(50), -- 'FIFO', 'LIFO'
  CreatedAt DATETIME
);

CREATE TABLE Categories (
  CategoryId UNIQUEIDENTIFIER PRIMARY KEY,
  Name NVARCHAR(255),
  Description NVARCHAR(MAX)
);

CREATE TABLE StockMovements (
  MovementId UNIQUEIDENTIFIER PRIMARY KEY,
  ProductId UNIQUEIDENTIFIER,
  MovementType NVARCHAR(50), -- 'In', 'Out'
  Quantity INT,
  ReferenceId UNIQUEIDENTIFIER, -- OrderId or ReceiptId
  MovementDate DATETIME
);

CREATE TABLE ProductBatches (
  BatchId UNIQUEIDENTIFIER PRIMARY KEY,
  ProductId UNIQUEIDENTIFIER,
  BatchNumber NVARCHAR(100),
  Quantity INT,
  ExpiryDate DATETIME,
  ReceivedDate DATETIME
);

CREATE TABLE Adjustments (
  AdjustmentId UNIQUEIDENTIFIER PRIMARY KEY,
  ProductId UNIQUEIDENTIFIER,
  Quantity INT,
  Type NVARCHAR(50), -- 'Damage', 'Audit', 'Correction'
  Reason NVARCHAR(MAX),
  AdjustmentDate DATETIME
);
```

---

### 4. **Finance Domain**

#### Purpose
Track cash, expenses, obligations, and maintain audit journals.

#### Key Entities
- **Shift** (Aggregate Root)
  - `ShiftId`, `ShiftNumber`, `Status` (Open, Closed)
  - `OpeningCash`, `ClosingCash`, `ExpectedBalance`
  - `OpenedAt`, `ClosedAt`

- **Expense** (Aggregate Root)
  - `ExpenseId`, `Amount`, `CategoryId`
  - `ExpenseDate`, `Description`

- **ExpenseCategory** (Reference)
  - Expense classification

- **FinancialObligation** (Aggregate Root)
  - `ObligationId`, `Type` (CustomerDebt, SupplierPayable)
  - `Amount`, `Status` (Active, Settled, Cancelled)
  - Settlement tracking

- **CashMovement** (Detail entity)
  - Deposits, withdrawals within shift

- **JournalEntry & JournalEntryLine** (Audit trail)
  - Double-entry accounting support
  - Automated from order events

#### Key Operations
```csharp
// Open shift
var cmd = new OpenShiftCommand(shiftNumber, openingCash: 5000.00m);

// Record cash movement
var cmd = new RecordCashMovementCommand(shiftId, amount: 100.00m, "Deposit");

// Record expense
var cmd = new RecordExpenseCommand(shiftId, amount: 50.00m, categoryId, "Supplies");

// Create obligation
var cmd = new CreateObligationCommand(customerId, amount: 200.00m, "Invoice", dueDate);

// Settle obligation
var cmd = new SettleObligationCommand(obligationId, paymentAmount: 100.00m);

// Cancel obligation
var cmd = new CancelObligationCommand(obligationId, reason);

// Close shift
var cmd = new CloseShiftCommand(shiftId, closingCash: 5150.00m);
```

#### Event Integration
- **Listens to**:
  - `OrderCompleted` → create ledger entry, track revenue
  - `CustomerPaymentReceived` → record cash, settle obligations
  - `PurchaseItemsReceived` → create supplier obligation
- **Publishes**: ShiftClosed, ObligationSettled (extensible)

#### Database Schema
```sql
-- Finance Domain Database
CREATE TABLE Shifts (
  ShiftId UNIQUEIDENTIFIER PRIMARY KEY,
  ShiftNumber NVARCHAR(100),
  Status NVARCHAR(50), -- 'Open', 'Closed'
  OpeningCash DECIMAL(18,2),
  ClosingCash DECIMAL(18,2),
  ExpectedBalance DECIMAL(18,2),
  OpenedAt DATETIME,
  ClosedAt DATETIME NULL
);

CREATE TABLE CashMovements (
  MovementId UNIQUEIDENTIFIER PRIMARY KEY,
  ShiftId UNIQUEIDENTIFIER,
  Amount DECIMAL(18,2),
  Type NVARCHAR(50), -- 'Deposit', 'Withdrawal'
  RecordedAt DATETIME
);

CREATE TABLE Expenses (
  ExpenseId UNIQUEIDENTIFIER PRIMARY KEY,
  ShiftId UNIQUEIDENTIFIER,
  CategoryId UNIQUEIDENTIFIER,
  Amount DECIMAL(18,2),
  Description NVARCHAR(MAX),
  ExpenseDate DATETIME
);

CREATE TABLE ExpenseCategories (
  CategoryId UNIQUEIDENTIFIER PRIMARY KEY,
  Name NVARCHAR(255)
);

CREATE TABLE FinancialObligations (
  ObligationId UNIQUEIDENTIFIER PRIMARY KEY,
  Type NVARCHAR(50), -- 'CustomerDebt', 'SupplierPayable'
  ReferenceId UNIQUEIDENTIFIER,
  Amount DECIMAL(18,2),
  SettledAmount DECIMAL(18,2),
  Status NVARCHAR(50), -- 'Active', 'Settled', 'Cancelled'
  DueDate DATETIME NULL,
  CreatedAt DATETIME
);

CREATE TABLE ObligationSettlements (
  SettlementId UNIQUEIDENTIFIER PRIMARY KEY,
  ObligationId UNIQUEIDENTIFIER,
  SettlementAmount DECIMAL(18,2),
  SettlementDate DATETIME
);

CREATE TABLE JournalEntries (
  EntryId UNIQUEIDENTIFIER PRIMARY KEY,
  ReferenceId UNIQUEIDENTIFIER,
  EntryDate DATETIME,
  Description NVARCHAR(MAX)
);

CREATE TABLE JournalEntryLines (
  LineId UNIQUEIDENTIFIER PRIMARY KEY,
  EntryId UNIQUEIDENTIFIER,
  Account NVARCHAR(100),
  DebitAmount DECIMAL(18,2),
  CreditAmount DECIMAL(18,2)
);
```

---

### 5. **Purchases Domain**

#### Purpose
Manage supplier relationships, purchase orders, and receipt handling.

#### Key Entities
- **Supplier** (Aggregate Root)
  - `SupplierId`, `Name`, `ContactInfo`, `PaymentTerms`

- **PurchaseOrder** (Aggregate Root)
  - `PurchaseOrderId`, `OrderNumber`, `SupplierId`
  - `Status` (Draft, Submitted, Received, Cancelled)
  - `TotalAmount`, `OrderDate`

- **PurchaseOrderItem** (Detail entity)
  - `ItemId`, `PurchaseOrderId`, `ProductId`
  - `OrderedQuantity`, `UnitCost`

- **PurchaseReceipt** (Aggregate Root)
  - Links received quantities to purchase order

- **PurchaseReceiptItem** (Detail entity)
  - Tracks received quantity per item

#### Key Operations
```csharp
// Create purchase order
var cmd = new CreatePurchaseOrderCommand(supplierId, items[], totalAmount);

// Submit purchase order
var cmd = new SubmitPurchaseCommand(purchaseOrderId);

// Receive purchase items (full or partial)
var cmd = new ReceivePurchaseCommand(purchaseOrderId, receivedItems[]);
```

#### Event Integration
- **Publishes**: `PurchaseItemsReceived` → updates inventory stock, creates finance obligation
- **Listens to**: None (initiator of events)

#### Database Schema
```sql
-- Purchases Domain Database
CREATE TABLE Suppliers (
  SupplierId UNIQUEIDENTIFIER PRIMARY KEY,
  Name NVARCHAR(255),
  ContactInfo NVARCHAR(255),
  PaymentTerms NVARCHAR(100),
  CreatedAt DATETIME
);

CREATE TABLE PurchaseOrders (
  PurchaseOrderId UNIQUEIDENTIFIER PRIMARY KEY,
  OrderNumber NVARCHAR(100) UNIQUE,
  SupplierId UNIQUEIDENTIFIER,
  Status NVARCHAR(50), -- 'Draft', 'Submitted', 'Received', 'Cancelled'
  TotalAmount DECIMAL(18,2),
  OrderDate DATETIME,
  ReceivedDate DATETIME NULL
);

CREATE TABLE PurchaseOrderItems (
  ItemId UNIQUEIDENTIFIER PRIMARY KEY,
  PurchaseOrderId UNIQUEIDENTIFIER,
  ProductId UNIQUEIDENTIFIER,
  OrderedQuantity INT,
  ReceivedQuantity INT DEFAULT 0,
  UnitCost DECIMAL(18,2)
);

CREATE TABLE PurchaseReceipts (
  ReceiptId UNIQUEIDENTIFIER PRIMARY KEY,
  PurchaseOrderId UNIQUEIDENTIFIER,
  ReceiptDate DATETIME,
  ReceivedBy NVARCHAR(255)
);

CREATE TABLE PurchaseReceiptItems (
  ReceiptItemId UNIQUEIDENTIFIER PRIMARY KEY,
  ReceiptId UNIQUEIDENTIFIER,
  OrderItemId UNIQUEIDENTIFIER,
  ReceivedQuantity INT,
  Condition NVARCHAR(50) -- 'Good', 'Damaged', 'Expired'
);
```

---

### 6. **Offers Domain**

#### Purpose
Manage promotional offers and pricing strategies.

#### Key Entities
- **Offer** (Aggregate Root)
  - `OfferId`, `Description`, `Type` (Discount, BundleDeal, BOGO)
  - `DiscountPercentage`, `DiscountAmount`
  - `StartDate`, `EndDate`, `IsActive`

- **OfferTarget** (Detail entity)
  - Segment targeting (customer, product, category)
  - Condition-based targeting

#### Key Operations
```csharp
// Create offer (extensible based on type)
var cmd = new CreateOfferCommand(description, type, discount, startDate, endDate);

// Activate/deactivate offer
var cmd = new UpdateOfferCommand(offerId, isActive: true);
```

#### Event Integration
- **Listens to**: OrderCompleted (for applicability checking)
- **Publishes**: OfferApplied (extensible)

#### Database Schema
```sql
-- Offers Domain Database
CREATE TABLE Offers (
  OfferId UNIQUEIDENTIFIER PRIMARY KEY,
  Description NVARCHAR(255),
  Type NVARCHAR(50), -- 'Discount', 'BundleDeal', 'BOGO'
  DiscountPercentage DECIMAL(5,2),
  DiscountAmount DECIMAL(18,2),
  StartDate DATETIME,
  EndDate DATETIME,
  IsActive BIT,
  CreatedAt DATETIME
);

CREATE TABLE OfferTargets (
  TargetId UNIQUEIDENTIFIER PRIMARY KEY,
  OfferId UNIQUEIDENTIFIER,
  TargetType NVARCHAR(50), -- 'Customer', 'Product', 'Category'
  ReferenceId UNIQUEIDENTIFIER
);
```

---

## API Endpoints & Integration

### HTTP REST Endpoints (Wolverine)

Wolverine provides convention-based HTTP endpoint routing with automatic validation middleware.

#### **Customers Endpoints**

```http
POST /customers
Content-Type: application/json

{
  "name": "John Doe",
  "phone": "+1-555-0100"
}
Response: 201 Created
{
  "customerId": "guid",
  "name": "John Doe",
  "phone": "+1-555-0100"
}
```

```http
POST /customers/{customerId}/payments
Content-Type: application/json

{
  "orderNumber": "ORD-001",
  "paidAmount": 150.00
}
Response: 200 OK
```

#### **Sales Endpoints**

```http
POST /orders
Content-Type: application/json

{
  "customerId": "guid"
}
Response: 201 Created
{
  "orderId": "guid",
  "orderNumber": "ORD-001",
  "status": "Pending"
}
```

```http
POST /orders/{orderNumber}/items
Content-Type: application/json

{
  "productId": "guid",
  "quantity": 5
}
Response: 200 OK
```

```http
DELETE /orders/{orderNumber}/items/{itemId}
Response: 204 No Content
```

```http
PUT /orders/{orderNumber}/items/{itemId}/quantity
Content-Type: application/json

{
  "quantity": 3
}
Response: 200 OK
```

#### **Inventory Endpoints**

```http
POST /products
Content-Type: application/json

{
  "name": "Product Name",
  "description": "Description",
  "categoryId": "guid"
}
Response: 201 Created
```

```http
PUT /products/{productId}
Content-Type: application/json

{
  "name": "Updated Name",
  "description": "Updated Description",
  "categoryId": "guid"
}
Response: 200 OK
```

```http
DELETE /products/{productId}
Response: 204 No Content
```

#### **Finance Endpoints**

```http
POST /shifts
Content-Type: application/json

{
  "shiftNumber": "SHIFT-001",
  "openingCash": 5000.00
}
Response: 201 Created
```

```http
POST /shifts/{shiftId}/expenses
Content-Type: application/json

{
  "categoryId": "guid",
  "amount": 50.00,
  "description": "Supplies"
}
Response: 200 OK
```

#### **Purchases Endpoints**

```http
POST /purchase-orders
Content-Type: application/json

{
  "supplierId": "guid",
  "items": [
	{ "productId": "guid", "quantity": 10, "unitCost": 25.50 }
  ]
}
Response: 201 Created
```

### OpenAPI/Swagger Documentation

**Access at**: `https://localhost:5001/swagger`

- Automatic documentation generation from endpoint handlers
- Try-it-out functionality for testing
- Request/response schema validation
- Authentication/Authorization documentation

---

## Messaging & Event Handling

### Wolverine Message Bus Architecture

Wolverine serves as the central nervous system for asynchronous communication:

```
┌─────────────────────────────────────────┐
│         Command Execution                │
│  (Handler invoked synchronously)         │
└─────────────────┬───────────────────────┘
				  │
				  ▼
┌─────────────────────────────────────────┐
│       Domain Event Published             │
│   (e.g., OrderCompleted event)           │
└─────────────────┬───────────────────────┘
				  │
		 ┌────────┴────────┐
		 │                 │
		 ▼                 ▼
	┌────────────┐  ┌──────────────┐
	│ SQL Server │  │ Message Bus  │
	│  (Outbox)  │  │  (In-Memory) │
	└────────────┘  └──────────────┘
		 │                 │
		 └────────┬────────┘
				  │
		 ┌────────▼────────────────┐
		 │  Event Handlers         │
		 │  (Multiple domains)     │
		 └─────────────────────────┘
```

### Event Types

#### **Domain Events** (Intra-Domain)
Events published and handled within the same bounded context.

```csharp
// Example: OrderCompleted published in Sales domain, 
// handled locally in Sales event handlers
public static class OrderCompletedEventHandler
{
	public static async Task Handle(OrderCompleted evt)
	{
		// Local business logic for order completion
	}
}
```

#### **Integration Events** (Inter-Domain)
Events published by one domain and listened to by others.

```csharp
// Sales publishes OrderCompleted
public record OrderCompleted(Guid OrderId, Guid CustomerId, decimal Amount, DateTime CompletedAt);

// Inventory listens and deducts stock
public static class OrderCompletedEventInventoryHandler
{
	public static async Task Handle(OrderCompleted evt, ISalesDataContext context)
	{
		// Deduct inventory
		var order = await context.Orders.FindAsync(evt.OrderId);
		foreach(var item in order.Items)
		{
			// Deduct from inventory
		}
	}
}

// Finance listens and records ledger
public static class OrderCompletedEventFinanceHandler
{
	public static async Task Handle(OrderCompleted evt, IFinanceDataContext context)
	{
		// Record journal entry, create obligation if needed
	}
}

// Customers listens and updates ledger
public static class OrderCompletedEventCustomerHandler
{
	public static async Task Handle(OrderCompleted evt, ICustomersDataContext context)
	{
		// Update customer ledger
	}
}
```

### Event Handlers by Domain

| Event | Published By | Handlers | Actions |
|-------|---|---|---|
| `OrderCompleted` | Sales | Inventory | Deduct stock |
| | | Finance | Record ledger entry |
| | | Customers | Update ledger |
| `OrderRefunded` | Sales | Inventory | Restore stock |
| | | Customers | Reverse debt |
| `PurchaseItemsReceived` | Purchases | Inventory | Add stock |
| | | Finance | Create obligation |
| `CustomerPaymentReceived` | Customers | Finance | Settle obligations |
| | | Sales | Track payment |

### Message Persistence & Reliability

**Transactional Outbox Pattern**:
```csharp
// All commands wrapped in transaction
using (var tx = await dbContext.Database.BeginTransactionAsync())
{
	// 1. Execute command (modify aggregate)
	var order = new Order(...);
	dbContext.Orders.Add(order);

	// 2. Publish event (written to outbox)
	await publisher.PublishAsync(new OrderCompleted(...));

	// 3. Commit both together
	await dbContext.SaveChangesAsync();
	await tx.CommitAsync();
}
// If any step fails, both are rolled back - no orphaned events
```

**SQL Server Message Store**:
- Wolverine persists all messages in `wolverine` schema tables
- Guarantees delivery even after crashes
- Message replay capability for failed handlers

---

## Database Design

### Multi-Database Strategy

Each domain maintains its own SQL Server database, enabling:
- **Data Isolation**: No cross-domain direct database access
- **Independent Scaling**: Can scale high-traffic domains independently
- **Technology Flexibility**: Each domain can use different storage technology
- **Clear Ownership**: Domain teams own their data model

### Database Contexts

```csharp
// Each domain has an IDataContext abstraction
public interface ICustomersDataContext
{
	DbSet<Customer> Customers { get; }
	DbSet<CustomerLedger> Ledgers { get; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

// Implemented by DbContext
public class CustomersDbContext : DbContext, ICustomersDataContext
{
	public DbSet<Customer> Customers { get; set; }
	public DbSet<CustomerLedger> Ledgers { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<Customer>()
			.HasMany(c => c.Ledgers)
			.WithOne(l => l.Customer);
	}
}
```

### Connection String Configuration

**appsettings.json**:
```json
{
  "ConnectionStrings": {
	"Constr": "Server=.;Database=SuperMarketDB;..."
  }
}
```

**For Multiple Databases** (if needed):
```json
{
  "ConnectionStrings": {
	"Customers": "Server=.;Database=CustomersDB;...",
	"Sales": "Server=.;Database=SalesDB;...",
	"Inventory": "Server=.;Database=InventoryDB;...",
	"Finance": "Server=.;Database=FinanceDB;...",
	"Purchases": "Server=.;Database=PurchasesDB;...",
	"Offers": "Server=.;Database=OffersDB;..."
  }
}
```

### Entity Framework Core Migrations

Each domain infrastructure project can manage migrations:

```powershell
# Add migration
dotnet ef migrations add AddCustomerTable --project Customers.Infrastructure

# Update database
dotnet ef database update --project Customers.Infrastructure

# Remove migration
dotnet ef migrations remove --project Customers.Infrastructure
```

### Query Optimization

- **Projections with GraphQL**: Avoid N+1 queries
- **Entity Framework Projections**: `.ProjectTo<DTO>()` from AutoMapper
- **Include Optimization**: Use `.Include()` strategically
- **Pagination**: Implemented at GraphQL level

---

## Error Handling & Validation

### Validation Pipeline

**Request Flow with Validation**:
```
HTTP Request
	↓
Wolverine Route Mapping
	↓
Fluent Validation Middleware
	├─ If Invalid → 400 Bad Request with validation errors
	└─ If Valid → Continue
	↓
Command Handler Execution
	├─ If Business Rule Violation → Throw CommandValidationException
	└─ If Success → Response
```

### Custom Exceptions

**CommandValidationException**:
```csharp
public class CommandValidationException : ApplicationException
{
	public Dictionary<string, List<string>> Errors { get; set; }

	public CommandValidationException(string message, 
		Dictionary<string, List<string>> errors) : base(message)
	{
		Errors = errors;
	}
}
```

**Usage**:
```csharp
if (customer.TotalDebt + orderTotal > DEBT_LIMIT)
{
	throw new CommandValidationException(
		"Customer debt limit exceeded",
		new Dictionary<string, List<string>>
		{
			{ "CustomerId", new() { "Debt limit exceeded" } }
		}
	);
}
```

### Global Exception Handler

**Centralized Exception Handling**:
```csharp
public class GlobalExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, 
		CancellationToken cancellationToken)
	{
		context.Response.ContentType = "application/problem+json";

		var response = new ProblemDetails();

		return exception switch
		{
			CommandValidationException cve =>
				await HandleValidationException(context, cve, response),

			InvalidOperationException ioe =>
				await HandleInvalidOperationException(context, ioe, response),

			_ => await HandleGeneralException(context, exception, response)
		};
	}
}
```

### Validation Examples

#### **Fluent Validation**:
```csharp
public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
	public CreateCustomerCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Name is required")
			.Length(2, 255).WithMessage("Name must be 2-255 characters");

		RuleFor(x => x.Phone)
			.Matches(@"^\+?1?\d{9,15}$").WithMessage("Invalid phone format");
	}
}
```

#### **Response Examples**:

**Success (200 OK)**:
```json
{
  "customerId": "550e8400-e29b-41d4-a716-446655440000",
  "name": "John Doe",
  "phone": "+1-555-0100"
}
```

**Validation Error (400 Bad Request)**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "0HMVJNLGS8NAC:00000001",
  "errors": {
	"name": ["Name is required"],
	"phone": ["Invalid phone format"]
  }
}
```

**Business Error (400 Bad Request)**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Command validation failed",
  "status": 400,
  "errors": {
	"CustomerId": ["Debt limit exceeded"]
  }
}
```

---

## Dependency Injection & Modularity

### IModule Pattern

All domains follow the IModule pattern for composable registration:

```csharp
public interface IModule
{
	string Name { get; }
	Assembly GetApplicationAssembly();
	Assembly GetPresentationAssembly();
	IServiceCollection RegisterModule(IServiceCollection services, 
		IConfiguration configuration);
	IApplicationBuilder UseModule(IApplicationBuilder app);
}
```

### Domain Module Implementation

```csharp
public sealed class CustomersModule : IModule
{
	public string Name => "Customers";

	public Assembly GetApplicationAssembly() => 
		typeof(ICustomersApplicationMarker).Assembly;

	public Assembly GetPresentationAssembly() => 
		typeof(CustomerEndpoints).Assembly;

	public IServiceCollection RegisterModule(IServiceCollection services, 
		IConfiguration configuration)
	{
		return services
			.AddCustomersInfrastructure(configuration)
			.AddCustomersPresentationServices()
			.AddCustomersGraphQLServices();
	}

	public IApplicationBuilder UseModule(IApplicationBuilder app) => app;
}
```

### Module Composition in Program.cs

```csharp
var modules = new List<IModule>
{
	new InventoryModule(),
	new SalesModule(),
	new CustomersModule(),
	new OffersModule(),
	new FinanceModule(),
	new PurchasesModule()
};

// Register all modules
builder.Services.AddModules(builder.Configuration, modules);

// Configure Wolverine with handler discovery per module
foreach (var module in modules)
{
	opts.Discovery.IncludeAssembly(module.GetPresentationAssembly());
	opts.Discovery.IncludeAssembly(module.GetApplicationAssembly());
}
```

### Dependency Injection Extension Methods

**Infrastructure Layer** (handles database and external service registration):
```csharp
public static IServiceCollection AddCustomersInfrastructure(
	this IServiceCollection services, 
	IConfiguration configuration)
{
	services.AddDbContext<ICustomersDataContext, CustomersDbContext>(options =>
		options.UseSqlServer(configuration.GetConnectionString("Constr")));

	services.AddScoped<ICustomerRepository, CustomerRepository>();

	return services;
}
```

**Application Layer**:
```csharp
public static IServiceCollection AddCustomersApplicationServices(
	this IServiceCollection services)
{
	services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(
		typeof(ICustomersApplicationMarker).Assembly));

	return services;
}
```

**Presentation Layer**:
```csharp
public static IServiceCollection AddCustomersPresentationServices(
	this IServiceCollection services)
{
	// Register GraphQL types, endpoints
	return services;
}
```

---

## Configuration & Environment Setup

### Application Settings

**appsettings.json** (Base configuration):
```json
{
  "Logging": {
	"LogLevel": {
	  "Default": "Information",
	  "Microsoft.AspNetCore": "Warning"
	}
  },
  "ConnectionStrings": {
	"Constr": "Server=.;Database=SuperMarketDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;Command Timeout=300;"
  }
}
```

**appsettings.Development.json** (Development overrides):
```json
{
  "Logging": {
	"LogLevel": {
	  "Default": "Debug",
	  "Microsoft": "Debug",
	  "System": "Debug"
	}
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
	"Constr": "Server=(localdb)\\mssqllocaldb;Database=SuperMarketDB_Dev;Trusted_Connection=True;"
  }
}
```

### Wolverine Configuration

**In Program.cs**:
```csharp
builder.Host.UseWolverine(opts =>
{
	// Enable runtime compilation for handler discovery
	opts.UseRuntimeCompilation();

	// Handle multiple handlers for same message type
	opts.MultipleHandlerBehavior = MultipleHandlerBehavior.Separated;

	// Enable FluentValidation validation
	opts.UseFluentValidation();

	// Set up EF Core transaction management
	opts.UseEntityFrameworkCoreTransactions()
		.WithDbContextAbstraction<IInventoryDataContext, InventoryDbContext>()
		.WithDbContextAbstraction<ISalesDataContext, SalesDbContext>()
		.WithDbContextAbstraction<ICustomersDataContext, CustomersDbContext>()
		.WithDbContextAbstraction<IOffersDataContext, OffersDbContext>()
		.WithDbContextAbstraction<IFinanceDataContext, FinanceDbContext>()
		.WithDbContextAbstraction<IPurchasesDataContext, PurchasesDbContext>();

	// Persist messages to SQL Server for durability
	opts.PersistMessagesWithSqlServer(connectionString, "wolverine");

	// Auto-create message storage (set to None for production)
	opts.AutoBuildMessageStorageOnStartup = JasperFx.AutoCreate.None;

	// JSON serialization
	opts.UseSystemTextJsonForSerialization();
});
```

### LaunchSettings.json

**Default Development Launch Settings**:
```json
{
  "profiles": {
	"https": {
	  "commandName": "Project",
	  "dotnetRunMessages": true,
	  "launchBrowser": true,
	  "launchUrl": "swagger",
	  "applicationUrl": "https://localhost:5001;http://localhost:5000",
	  "environmentVariables": {
		"ASPNETCORE_ENVIRONMENT": "Development"
	  }
	}
  }
}
```

### Environment Variables

For sensitive data in production:

```powershell
# Set connection string from environment
$env:ConnectionStrings__Constr = "Server=prod-server;Database=SuperMarket;..."

# Or via Azure Key Vault
$env:ASPNETCORE_ENVIRONMENT = "Production"
```

---

## GraphQL Support

### Hot Chocolate GraphQL Integration

GraphQL provides an alternative query interface with strong typing and schema introspection.

**Server Configuration**:
```csharp
builder.Services.AddGraphQLServer()
	.RegisterDbContextFactory<CustomersDbContext>()
	.AddProjections()
	.AddPagingArguments()
	.AddSorting()
	.AddFiltering();
```

### GraphQL Types (Example: Customers)

```csharp
// GraphQL Object Type
public class CustomerType
{
	public Guid CustomerId { get; set; }
	public string Name { get; set; }
	public string Phone { get; set; }
	public decimal TotalDebt { get; set; }
	public decimal TotalPaid { get; set; }

	[GraphQLType(typeof(ListType<CustomerLedgerType>))]
	public IEnumerable<CustomerLedger> Ledgers { get; set; }
}

// Resolver
[QueryType]
public class CustomerQueries
{
	[UsePaging]
	[UseFiltering]
	[UseSorting]
	public IQueryable<CustomerType> GetCustomers(
		[Service] ICustomersDataContext context)
	{
		return context.Customers
			.AsNoTracking()
			.ProjectTo<CustomerType>();
	}
}
```

### GraphQL Queries

**Query Example**:
```graphql
query {
  customers(first: 10, after: "cursor") {
	edges {
	  node {
		customerId
		name
		phone
		totalDebt
		ledgers {
		  amount
		  type
		  transactionDate
		}
	  }
	}
	pageInfo {
	  hasNextPage
	  endCursor
	}
  }
}
```

**Mutation Example** (if mutations implemented):
```graphql
mutation {
  createCustomer(input: { name: "Jane Doe", phone: "+1-555-0101" }) {
	customer {
	  customerId
	  name
	  phone
	}
  }
}
```

### Endpoint

Access GraphQL Playground at: `https://localhost:5001/graphql`

---

## Best Practices & Patterns

### 1. **Aggregate Design**
- **Single Responsibility**: Each aggregate root manages one business concept
- **Consistency Boundary**: All changes to related entities go through aggregate root
- **Identity**: Each aggregate has unique identifier (Guid)

```csharp
public class Order  // Aggregate Root
{
	public Guid OrderId { get; set; }  // Aggregate identity
	public string OrderNumber { get; set; }

	private List<OrderItem> _items = new();  // Private collection
	public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

	public void AddItem(Guid productId, int quantity, decimal price)
	{
		// Business rule: Ensure inventory availability before adding
		var item = new OrderItem(...);
		_items.Add(item);
	}
}
```

### 2. **Domain Events**
- **Publish After Persistence**: Events published in transaction scope
- **Event Naming**: Use past tense (OrderCompleted, not OrderComplete)
- **Rich Event Data**: Include all necessary context in event

```csharp
public record OrderCompleted(
	Guid OrderId, 
	Guid CustomerId, 
	decimal TotalAmount,
	IEnumerable<OrderItemData> Items,  // Rich context
	DateTime CompletedAt);

// Publish in handler
var evt = new OrderCompleted(order.OrderId, order.CustomerId, 
	order.TotalAmount, order.Items, DateTime.UtcNow);
await publisher.PublishAsync(evt);
```

### 3. **Command Handlers**
- **Single Responsibility**: One handler per command
- **Validation First**: Validate before modifying state
- **Publish Events Last**: Events published as final step

```csharp
public static class StartOrderCommandHandler
{
	public static async Task<OrderResult> Handle(
		StartOrderCommand command,
		ISalesDataContext context)
	{
		// 1. Validate
		var customer = await context.Customers
			.FindAsync(command.CustomerId);
		if (customer == null)
			throw new CommandValidationException(...);

		// 2. Execute business logic
		var order = Order.Start(command.CustomerId);
		context.Orders.Add(order);

		// 3. Persist
		await context.SaveChangesAsync();

		// 4. Publish events
		await context.PublishAsync(new OrderStarted(order.OrderId, ...));

		return new OrderResult { OrderId = order.OrderId };
	}
}
```

### 4. **Event Handlers**
- **Eventual Consistency**: Don't assume immediate consistency
- **Idempotency**: Handle duplicate event delivery gracefully
- **Error Handling**: Log and alert on handler failures

```csharp
public static class OrderCompletedEventInventoryHandler
{
	public static async Task Handle(
		OrderCompleted evt,
		IInventoryDataContext context)
	{
		// Idempotent check: Skip if already processed
		var existing = await context.StockMovements
			.FirstOrDefaultAsync(m => m.ReferenceId == evt.OrderId 
				&& m.MovementType == "Out");
		if (existing != null)
			return;  // Already processed

		// Process event
		var order = await context.Orders.FindAsync(evt.OrderId);
		foreach (var item in order.Items)
		{
			// Create stock movement
		}

		await context.SaveChangesAsync();
	}
}
```

### 5. **Query Optimization**
- **Projection to DTOs**: Transform in database, not in memory
- **Explicit Loading**: Use `.Include()` only for needed relationships
- **Pagination**: Always paginate large result sets

```csharp
// ❌ Bad: Loads all data, filters in memory
var allCustomers = await context.Customers.ToListAsync();
var page = allCustomers.Skip(10).Take(20).ToList();

// ✅ Good: Filters and paginates at database level
var page = await context.Customers
	.OrderBy(c => c.CustomerId)
	.Skip(10)
	.Take(20)
	.Select(c => new CustomerDto
	{
		CustomerId = c.CustomerId,
		Name = c.Name,
		LedgerCount = c.Ledgers.Count
	})
	.ToListAsync();
```

### 6. **Error Response Consistency**
- **Problem Details Format**: RFC 7807 compliant
- **Detailed Error Information**: Include error codes and paths
- **Correlation Tracking**: TraceId for debugging

```json
{
  "type": "https://api.example.com/errors/validation-error",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "detail": "The request contains invalid data.",
  "traceId": "0HMVJNLGS8NAC:00000001",
  "errors": {
	"customerId": ["Customer not found"],
	"orderTotal": ["Amount must be positive"]
  }
}
```

### 7. **Testing Strategy** (Extensible)

```csharp
[TestClass]
public class StartOrderCommandHandlerTests
{
	private IServiceProvider _services;

	[TestInitialize]
	public void Setup()
	{
		var collection = new ServiceCollection();
		collection.AddSalesServices();
		_services = collection.BuildServiceProvider();
	}

	[TestMethod]
	public async Task Handle_WithValidCustomer_CreatesOrder()
	{
		// Arrange
		var command = new StartOrderCommand(customerId: Guid.NewGuid());
		var handler = _services.GetRequiredService<ICommandHandler<StartOrderCommand>>();

		// Act
		var result = await handler.Handle(command);

		// Assert
		Assert.IsNotNull(result.OrderId);
	}
}
```

### 8. **Logging & Observability**
- **Structured Logging**: Use ILogger with structured fields
- **Business Events**: Log important domain events
- **Performance Metrics**: Track command execution time

```csharp
public static class CreateCustomerCommandHandler
{
	public static async Task<CustomerResult> Handle(
		CreateCustomerCommand command,
		ILogger<CreateCustomerCommandHandler> logger,
		ICustomersDataContext context)
	{
		logger.LogInformation(
			"Creating customer {@Command}",
			command);

		var customer = new Customer(command.Name, command.Phone);
		context.Customers.Add(customer);
		await context.SaveChangesAsync();

		logger.LogInformation(
			"Customer created {@Customer}",
			new { customer.CustomerId, customer.Name });

		return new CustomerResult { CustomerId = customer.CustomerId };
	}
}
```

### 9. **Transaction Management**
- **Explicit Transactions**: Use when coordinating multiple operations
- **Savepoints**: Useful for partial rollback scenarios
- **Distributed Transactions**: Handled via events (Saga pattern)

```csharp
using (var transaction = await context.Database.BeginTransactionAsync())
{
	try
	{
		var order = new Order(...);
		context.Orders.Add(order);

		// Multiple operations in transaction
		var customer = await context.Customers.FindAsync(customerId);
		customer.UpdateLedger(order.TotalAmount);

		await context.SaveChangesAsync();
		await transaction.CommitAsync();
	}
	catch
	{
		await transaction.RollbackAsync();
		throw;
	}
}
```

### 10. **Backward Compatibility**
- **API Versioning**: Support multiple API versions
- **Event Versioning**: Handle schema evolution
- **Deprecation Policy**: Clear deprecation timeline

```csharp
// Support v1 (deprecated) and v2 (current)
[ApiVersion("1.0", Deprecated = true)]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/customers")]
public class CustomerController : ControllerBase
{
	[HttpGet("{id}")]
	public async Task<ActionResult<CustomerDto>> Get(Guid id)
	{
		// Version-specific behavior
	}
}
```

---

## Development Workflow

### Adding a New Feature

**Example: Add "Customer Tier Management" (VIP/Regular)**

#### 1. **Domain Layer** (Customers.Domain)
```csharp
public enum CustomerTier { Regular, VIP, Gold }

public class Customer
{
	public CustomerTier Tier { get; private set; }

	public void UpgradeTier(CustomerTier newTier)
	{
		if (newTier < Tier)
			throw new InvalidOperationException("Cannot downgrade tier");

		Tier = newTier;
		this.AddDomainEvent(new CustomerTierUpgraded(this.CustomerId, newTier));
	}
}

public record CustomerTierUpgraded(Guid CustomerId, CustomerTier NewTier);
```

#### 2. **Application Layer** (Customers.Application)
```csharp
public record UpgradeCustomerTierCommand(Guid CustomerId, CustomerTier NewTier);

public static class UpgradeCustomerTierCommandHandler
{
	public static async Task<Result> Handle(
		UpgradeCustomerTierCommand command,
		ICustomersDataContext context)
	{
		var customer = await context.Customers.FindAsync(command.CustomerId)
			?? throw new CommandValidationException("Customer not found");

		customer.UpgradeTier(command.NewTier);
		await context.SaveChangesAsync();

		return Result.Success();
	}
}
```

#### 3. **Presentation Layer** (Customers.Presentation)
```csharp
public class UpgradeCustomerTierEndpoint
{
	[WolverinePost("/customers/{customerId}/tier")]
	public async Task Post(Guid customerId, UpgradeTierRequest request)
	{
		var command = new UpgradeCustomerTierCommand(customerId, request.NewTier);
		await Mediator.SendAsync(command);
	}
}
```

#### 4. **Infrastructure Layer** (Customers.Infrastructure)
```csharp
// Add migration
builder.Entity<Customer>()
	.Property(c => c.Tier)
	.HasConversion<string>()
	.HasDefaultValue(CustomerTier.Regular);
```

---

## Troubleshooting & Common Issues

### Issue: "DbContext not registered"
**Solution**: Ensure `AddCustomersInfrastructure()` is called in `AddModules()`.

### Issue: "Event handler not executing"
**Solution**: 
1. Verify handler class is `public static`
2. Ensure handler is in discovered assembly
3. Check Wolverine handler discovery logging

### Issue: "Connection timeout"
**Solution**: 
- Increase `Command Timeout` in connection string
- Check SQL Server availability
- Verify connection string format

### Issue: "Validation errors not detailed"
**Solution**: Check GlobalExceptionHandler is registered and validation results are mapped correctly.

---

## Resources & References

- **Wolverine Docs**: https://wolverine.io
- **Hot Chocolate GraphQL**: https://chillicream.com/docs/hotchocolate
- **Entity Framework Core**: https://docs.microsoft.com/ef/core/
- **Domain-Driven Design**: Eric Evans - "Domain-Driven Design" book
- **Microservices Patterns**: Sam Newman - "Building Microservices"

---

## Contributing & Support

For issues, enhancements, or questions:
1. Open an issue on GitHub
2. Create a feature branch (`feature/your-feature`)
3. Submit a pull request with clear description
4. Ensure all tests pass and code follows conventions

---

## License

This project is part of the RMS (Repository Management System) initiative. Please refer to the repository LICENSE file for details.

---

## Summary

**Market System Demo** demonstrates enterprise-grade architecture with:
- ✅ Clean Architecture with 4-layer separation
- ✅ Domain-Driven Design with aggregates and value objects
- ✅ CQRS pattern with Wolverine message bus
- ✅ Event-driven inter-domain communication
- ✅ GraphQL and REST API support
- ✅ Comprehensive error handling and validation
- ✅ Modular, composable design with IModule pattern
- ✅ Production-ready patterns (transactional outbox, idempotency, etc.)
- ✅ .NET 10 with modern async/await throughout

The system is designed for **scalability**, **maintainability**, and **extensibility**, making it ideal for learning advanced .NET patterns and enterprise architecture.
