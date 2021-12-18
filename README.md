# DevelopmentFast.Cache
Uma biblioteca para ajudar no rápido desenvolvimento para aplicar camada de Cache utilizando Redis, usando Repository Pattern.

<p align="center">
  <img src="https://github.com/ABNERMATHEUS/DevelopmentFast.Cache/blob/master/Logo.svg" width="320" alt="Logo" /></a>
</p>

# Development Fast - Cache


| Package |  Version | Popularity |
| ------- | ----- | ----- |
| `DevelopmentFast.Cache` | [![NuGet](https://img.shields.io/nuget/v/DevelopmentFast.CacheRedis.svg)](https://www.nuget.org/packages/DevelopmentFast.CacheRedis) | [![Nuget](https://img.shields.io/nuget/dt/DevelopmentFast.CacheRedis.svg)](https://www.nuget.org/packages/DevelopmentFast.CacheRedis) |

### O que é ?

<aside>
📌 É uma biblioteca em .NET com a responsabilidade ajudar no rápido desenvolvimento para aplicar camada de Cache utilizando Redis, usando Repository Pattern.
</aside>

### Por que eu criei ?

<aside>
📌 Para facilitar o desenvolvimento e economizar o tempo que eu tinha de configurar uma camada de cache para aplicar em qualquer lugar do projeto.

</aside>

### Onde está disponível ?

<aside>
📌 Está disponível no .Nuget.org
  
  <br/>
  <a href="https://www.nuget.org/packages/DevelopmentFast.CacheRedis">Link do Nuget</a>
    <br/>
  
  `Install-Package DevelopmentFast.CacheRedis -Version 1.0.0`
  
  

</aside>


### Como utilizar ?


### Injeções de dependência e conexão

```csharp
builder.Services.AddSingleton<IDistributedCache, RedisCache>();
builder.Services.AddStackExchangeRedisCache(x => x.Configuration = "localhost:6379");
```

### Construtor

```csharp
private readonly IBaseRedisRepositoryDF _baseRedisRepositoryDF;

public StudentController(IBaseRedisRepositoryDF baseRedisRepositoryDF)
{
    this._baseRedisRepositoryDF = baseRedisRepositoryDF;
}
```

### Criar ou Atualizar assíncrono

```csharp
var student = new Student("name_student");
await _baseRedisRepositoryDF.CreateOrUpdateAsync<Student>("key_example", student , TimeSpan.FromMinutes(1));
```

### Criar ou Atualizar síncrono

```csharp
var student = new Student("name_student");
_baseRedisRepositoryDF.CreateOrUpdate<Student>("key_example", student , TimeSpan.FromMinutes(1));
```

### Buscar assíncrono

```csharp
var student = await _baseRedisRepositoryDF.GetAsync<Student>("key_example");
```

### Buscar síncrono

```csharp
var student =  _baseRedisRepositoryDF.Get<Student>("key_example");
```
