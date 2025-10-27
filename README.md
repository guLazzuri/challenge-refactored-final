# Challenge Mottu - Clean Architecture API (CP5)

## 📋 Descrição do Domínio

Este projeto implementa uma API para gerenciamento de veículos e usuários, seguindo os princípios de **Clean Architecture**, **Domain-Driven Design (DDD)** e **Clean Code**. O domínio principal é o gerenciamento de uma frota de veículos para locação, com persistência em **MongoDB**, incorporando **Health Check** e **Versionamento de API** via Swagger.

- **Gestão de Veículos**: Cadastro, consulta, atualização e controle de status dos veículos
- **Gestão de Usuários**: Cadastro e gerenciamento de usuários do sistema
- **Histórico de Manutenção**: Controle de manutenções preventivas e corretivas dos veículos

## 🏗️ Arquitetura

O projeto segue a **Clean Architecture** com separação clara de responsabilidades:

```
📦 src
 ┣ 📂 Api             -> Controllers, configurações da API, Swagger, Health Check
 ┣ 📂 Application     -> Casos de uso, DTOs, Services
 ┣ 📂 Domain          -> Entidades, Value Objects, Interfaces, Agregados
 ┗ 📂 Infrastructure  -> Acesso a dados (MongoDB), repositórios
```

### Camadas

- **Domain**: Contém as regras de negócio, entidades ricas e interfaces
- **Application**: Orquestra os casos de uso e transformações de dados
- **Infrastructure**: Implementa persistência e acesso a dados (**MongoDB**)
- **Api**: Expõe endpoints REST, versionamento e configurações de observabilidade

## 🎯 Conceitos DDD Implementados

### Entidades Ricas (Migradas para MongoDB)
- **Vehicle**: Entidade com comportamentos como `Rent()`, `Return()`, `SendToMaintenance()`
- **User**: Entidade com comportamentos como `Activate()`, `Deactivate()`, `RentVehicle()`
- **MaintenanceHistory**: Entidade para controle de manutenções

### Value Objects
- **VehicleModel**: Encapsula marca e modelo do veículo
- **Email**: Validação e normalização de emails

### Agregado Raiz
- **VehicleAggregate**: Controla a consistência entre veículo e histórico de manutenção

### Interfaces de Repositório
- Definidas no domínio para inversão de dependência
- Implementadas na camada de infraestrutura com o **repositório genérico `IMongoRepository<T>`**

## 🚀 Como Executar

### Pré-requisitos
- .NET 8.0 SDK
- **MongoDB** (local ou MongoDB Atlas)

### Passos para execução

1. **Clone o repositório**
```bash
git clone <url-do-repositorio>
cd challenge-refactored
```

2. **Configurar o MongoDB**
   - Certifique-se de que o MongoDB esteja em execução.
   - Atualize a seção `MongoDbSettings` no `src/Api/appsettings.json` com suas credenciais, se necessário.

```json
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "ChallengeMottuDB"
  }
```

3. **Restaurar dependências**
```bash
dotnet restore
```

4. **Executar a aplicação**
```bash
cd src/Api
dotnet run
```

5. **Acessar o Swagger (Documentação da API)**
- Abra o navegador em: `https://localhost:5001` ou `http://localhost:5000`
- A documentação interativa estará disponível na página inicial, com as versões **v1** e **v2** (exemplo de versionamento).

6. **Acessar o Health Check**
- Endpoint: `/health`
- Verifica a saúde da aplicação e a conectividade com o **MongoDB**.

## 📚 Exemplos de Requisições (v1)

Os endpoints agora estão versionados com `/api/v1/...`

### Criar Veículo
```http
POST /api/v1/vehicles
Content-Type: application/json

{
  "licensePlate": "ABC1234",
  "brand": "Honda",
  "model": "CB 600F",
  "year": 2023
}
```

### Listar Veículos Disponíveis
```http
GET /api/v1/vehicles/available
```

### Criar Usuário
```http
POST /api/v1/users
Content-Type: application/json

{
  "name": "João Silva",
  "email": "joao@email.com",
  "document": "12345678901",
  "type": 1
}
```

### Alterar Status do Veículo
```http
PATCH /api/v1/vehicles/{id}/status
Content-Type: application/json

{
  "status": 2
}
```

**Status disponíveis:**
- 1: Available (Disponível)
- 2: Rented (Alugado)
- 3: InMaintenance (Em Manutenção)
- 4: Inactive (Inativo)

## 🧪 Testes

Para executar os testes (quando implementados):
```bash
dotnet test
```

## 📦 Tecnologias Utilizadas

- **.NET 8.0**
- **MongoDB.Driver**
- **AspNetCore.HealthChecks.MongoDb**
- **Microsoft.AspNetCore.Mvc.Versioning**
- **Swagger/OpenAPI**
- **ASP.NET Core Web API**

## 👥 Integrantes do Grupo

- [Gustavo Ramires Lazzuri] - RM[556772]
- [Mateus Henrique de Souza] - RM[558424]
- [Cauan Aranega S Passos] - RM[555466]




## 📄 Licença

Este projeto foi desenvolvido para fins acadêmicos como parte do Checkpoint 5 da disciplina de Clean Code, DDD e Clean Architecture.

