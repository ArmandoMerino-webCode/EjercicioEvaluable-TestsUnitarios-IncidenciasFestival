namespace FestivalIncidencias.Api.Services;

public enum TipoErrorIncidencias
{
    Validacion,
    NoEncontrado,
    Conflicto
}

public sealed class ErrorIncidenciasException : Exception
{
    public TipoErrorIncidencias Tipo { get; }

    public ErrorIncidenciasException(
        TipoErrorIncidencias tipo,
        string mensaje) : base(mensaje)
    {
        Tipo = tipo;
    }
}
