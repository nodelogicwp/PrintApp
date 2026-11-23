using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PunktDruku.ViewModels;

/// <summary>
/// Klasa bazowa wzorca MVVM - powiadamia widok o zmianach właściwości.
/// </summary>
public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>
    /// Ustawia pole i zgłasza zmianę. Zwraca true, gdy wartość faktycznie się zmieniła.
    /// </summary>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
