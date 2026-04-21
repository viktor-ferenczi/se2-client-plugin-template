# Settings UI

The template ships with an attribute-driven Settings UI generator. Mark the
properties on `Config` with one of the built-in attributes and a matching row
will be rendered automatically in the settings dialog. The dialog is opened by
Pulsar when the user clicks the plugin's settings button.

## Supported attributes

| Attribute              | Property type                | Description                                                        |
|------------------------|------------------------------|--------------------------------------------------------------------|
| `[Checkbox]`           | `bool`                       | Toggle.                                                            |
| `[Slider(min, max, step, type)]` | `int`, `float`, `double` | Slider with optional integer/float formatting.               |
| `[Textbox]`            | `string`                     | Single-line text input.                                            |
| `[Dropdown]`           | any `enum`                   | Combo box with one entry per enum value (CamelCase → "Camel Case").|
| `[Color]`              | `Avalonia.Media.Color`       | Color swatch + hex text box. Set `hasAlpha: true` for RGBA.        |
| `[Keybind]`            | `Binding`                    | Button that opens SE2's input composition dialog; right-click clears.|
| `[Button]`             | `void` method                | Renders a button that invokes the method.                          |
| `[Separator("caption")]` | any property               | Inserts a labelled separator above the property.                   |

All attributes accept an optional `label` (overrides the property name) and
`description` (used as a tooltip). See [Config.cs](../Config.cs) for
working examples of every attribute.

## Color property persistence

`Avalonia.Media.Color` is not XML-serializable, so `[XmlIgnore]` the public
`Color` property and back it with a packed ARGB `uint` plus a companion public
property that the XML serializer can see:

```csharp
private uint color = 0xFF00FFFFu;

[XmlIgnore]
[Color(description: "RGB color")]
public Color Color
{
    get => Avalonia.Media.Color.FromUInt32(color | 0xFF000000u);
    set => SetField(ref color, value.ToUInt32() | 0xFF000000u);
}

// Serialized to XML instead of the Color property.
public uint ColorPacked { get => color; set => color = value; }
```

## Keybind persistence

`Binding` stores a Windows virtual-key code plus Ctrl/Alt/Shift flags — the same
representation SE2's `KeyboardInputs` uses — so it round-trips cleanly through
`InputControlComposer.KeyboardDefault` and XML-serializes as plain fields.
`Binding.ToInputControl(action)` and `Binding.FromInputControl(control)` bridge
to the game's `InputControl` type when you need to register the binding with
SE2's input system.

## Customizing layout

Dialog size and the width of the label column are defined in
[SettingsLayout.cs](SettingsLayout.cs). Adjust `DialogWidth`, `DialogHeight`,
and `LabelColumnWidth` to fit the amount of content and the fonts used by your
plugin.

For deeper customization (fonts, colors, spacing) edit
[SettingsScreen.cs](SettingsScreen.cs) and
[RowBuilder.cs](Elements/RowBuilder.cs).

## Adding your own control types

To support a property type that is not covered by the built-in attributes,
add a new file under [Elements/](Elements/) that implements `IElement` and
derives from `Attribute`. `BuildRow` should return a `Control` built via
`RowBuilder.NewRow(label, tooltip, ...)`.
[`SettingsGenerator`](SettingsGenerator.cs) will pick the new attribute up
automatically — no registration needed.
