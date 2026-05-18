# Troubleshooting

## Allagma eval POST fails

Restart Allagma with:

```powershell
$env:Allagma__Evaluation__ManualWriteEnabled="true"
```

## Conexus route evidence missing

Use fake provider first. Confirm project key:

```text
cx-dev-key-change-me
```

## Frontend dashboard empty

Expected if no recent evaluated runs. Use:

```text
/allagma/evaluations?dashboardFixture=ci-suite
```

## Port in use

```powershell
netstat -ano | findstr :5081
netstat -ano | findstr :5082
netstat -ano | findstr :5083
```
