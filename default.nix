let
  pkgs = import <nixpkgs> {};
in
  pkgs.buildDotnetModule rec {
    pname = "Neuro-recall-reverie";
    version = "0.0.1";
    src = ./.;

    projectFile = "NeuroBdayJam/NeuroBdayJam.csproj";
    nugetDeps = ./deps.nix;

    dotnet-sdk = pkgs.dotnetCorePackages.sdk_7_0;
    dotnet-runtime = pkgs.dotnetCorePackages.runtime_7_0;

    runtimeDeps = with pkgs; [
      xorg.libX11
      xorg.libXi
      glfw
      libGLU
      libGL
      alsa-lib
    ];

    nativeBuildInputs = [
      pkgs.makeWrapper
    ];

    dotnetInstallFlags = ["--framework" "net7.0"];

    fixupPhase = ''
      mkdir -p $out/bin/Resources/
      cp ./NeuroBdayJam/Resources/ResourceFiles/*.dat $out/bin/Resources/

      wrapProgram $out/bin/NeuroRecallReverie \
        --set NEURO_RECALL_REVERIE_RESOURCES $out/bin/Resources/
    '';
  }
