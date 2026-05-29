# Aisthesis ReleaseMode runbook

## Recommended local RC mode

```powershell
$env:ConnectionStrings__Aisthesis = "Host=localhost;Port=5432;Database=aisthesis;Username=aisthesis;Password=aisthesis"
$env:Aisthesis__Auth__Mode = "producer-token"
$env:Aisthesis__Auth__Producers__allagma__Token = "dev-allagma"
$env:Aisthesis__Auth__Producers__allagma__CanWrite = "true"
$env:Aisthesis__Auth__Producers__kanon__Token = "dev-kanon"
$env:Aisthesis__Auth__Producers__kanon__CanWrite = "true"
$env:Aisthesis__Auth__Producers__conexus__Token = "dev-conexus"
$env:Aisthesis__Auth__Producers__conexus__CanWrite = "true"
$env:Aisthesis__Auth__Producers__metabole__Token = "dev-metabole"
$env:Aisthesis__Auth__Producers__metabole__CanWrite = "true"
```

Do not use fake evidence for live PASS claims.
