#  Serviço de Autenticação e Tarefas - Arquitetura SOA (Global Solution)

O projeto demonstra a implementação de uma arquitetura modular baseada em serviços e a aplicação de políticas de segurança stateless (sem estado) conforme os requisitos de Serviços Orientados a Arquitetura (SOA).

##  Membros do Grupo
* Bruno Venturi Lopes Vieira - 99431
* Guilherme Alves de Lima - 550433
* Leonardo de Oliveira Ruiz - 98901

## 🔒 Provas de Segurança e Arquitetura SOA

| **Sessão Stateless com JWT** | O sistema utiliza **JSON Web Tokens (JWT)** para autenticação, garantindo que o servidor não armazene o estado da sessão do usuário. O token é gerado no login e validado em cada requisição. | `AuthService.cs`, `Program.cs` |
| **Autorização (Perfis/Roles)** | O token JWT é gerado com a *Claim* de Perfil (`Perfil: "Admin"`), permitindo que regras de acesso futuras sejam aplicadas via `[Authorize(Roles="Admin")]`. | `AuthService.cs`, `Usuario.cs` |
| **Regras de Negócio como Serviços** | A lógica de negócios para Tarefas é isolada na camada de Serviço, garantindo a modularidade e a reutilização (`builder.Services.AddScoped`). | `ITarefaService.cs`, `TarefaService.cs` |
| **Tratamento Global de Exceções** | Implementação de um Middleware que captura exceções em nível de pipeline e retorna respostas JSON padronizadas (equivalente ao *ControllerAdvice*). | `GlobalErrorMiddleware.cs` |

## 🔑 Endpoints de Autenticação
Acesse o Swagger UI para testar a sequência completa:
1. `POST /auth/register` (Cria usuário)
2. `POST /auth/login` (Gera o Token JWT)
3. Acessa `GET /api/v1/tarefas` (Com Token no Header)

***
**Com a conclusão desses documentos, o seu projeto está pronto para ser entregue nas duas disciplinas, cobrindo todos os requisitos.**
