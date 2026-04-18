using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClientPlugin;

public class Config : INotifyPropertyChanged
{
    #region Options

    // TODO: Define your configuration options and their default values
    private bool enabled = true;

    #endregion

    #region Properties

    // TODO: Define a property for each of the option fields with the same type
    public bool Enabled
    {
        get => enabled;
        set => SetField(ref enabled, value);
    }

    #endregion

    #region Property change notification boilerplate

    public static readonly Config Default = new Config();
    public static Config Current = ConfigStorage.Load();

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion
}
