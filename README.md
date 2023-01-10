[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![Build Status][build-shield]][build-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/optsoldev/components-backend-core.svg
[contributors-url]: https://github.com/optsoldev/components-backend-core/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/optsoldev/components-backend-core.svg
[forks-url]: https://github.com/optsoldev/components-backend-core/network/members
[stars-shield]: https://img.shields.io/github/stars/optsoldev/components-backend-core.svg
[stars-url]: https://github.com/optsoldev/components-backend-core/stargazers
[issues-shield]: https://img.shields.io/github/issues/optsoldev/components-backend-core.svg
[issues-url]: https://github.com/optsoldev/components-backend-core/issues
[license-shield]: https://img.shields.io/github/license/optsoldev/components-backend-core.svg
[license-url]: https://github.com/optsoldev/components-backend-core/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/company/optsoltecnologia/
[product-screenshot]: images/screenshot.png
[build-shield]: https://dev.azure.com/optsoldev/OPTSOL%20Components%20Backend/_apis/build/status/optsoldev.components-backend-core?branchName=main
[build-url]: https://dev.azure.com/optsoldev/OPTSOL%20Components%20Backend/_build/latest?definitionId=4&branchName=main

## Sample

Requisitos para rodar a sample: 

Um banco SQL. Você pode rodar o comando abaixo com o docker instalado para rodar uma instancia do azure-sql-edge. 
```
docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=OPTSOL@dev' -p 1433:1433 --name azuresqledge -d mcr.microsoft.com/azure-sql-edge
```

Rodar a migration

``dotnet ef database update --context PlaygroundContext``

Lembre-se de rodar o Optsol.Playground.Api com ``"ASPNETCORE_ENVIRONMENT": "Development"``.

## Changes

### 2.2.1

- ApiControllerBase: Removido os atributos de Authorize. Fica como responsabilidade do desenvolvedor adicionar na controller que herda. Seguir o exmeplo do Playground.
- AddSwagger: Removido a extension.

### 2.3.0

#### Optsol.Components.Infra.Security.AzureB2C 

- Ajustes no OptsolAuthorize para receber claims corretamente.
- Adicionado propriedade no SecuritySettings para o nome da claim de segurança **SecurityClaim**.
- Adicionado propriedade no SecuritySettings para desenvolvimento **DevelopmentClaims**, um array de claims para testes.

#### Optsol.Components.Service

- Simplificação na extension AddCors e UseCors para utilização de uma DefaultPolicy e outras Policies. 

### 2.3.3

- Criação do Interceptor para o uso de Tenant na SDK.
- Melhoria do RepositoryOptions para injetar esse interceptor e permitir outros serem adicionados.
- Criação do LoggedUser para leitura dos tokens. 

Teste 6