using shortid;
using shortid.Configuration;

namespace Ase.Doc.Demo.Services;

public static class IdService
{
  public static string Create() => ShortId.Generate(new GenerationOptions
  {
    UseSpecialCharacters = false,
    UseNumbers = true,
    Length = 10
  });
}
