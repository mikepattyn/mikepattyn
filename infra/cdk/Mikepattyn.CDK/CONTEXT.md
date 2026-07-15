# CDK Deploy App

## Language

**DeployApp**:
The `Mikepattyn.CDK` executable project. It wires stack instances together and calls `app.Synth()`.
_Avoid_: defining reusable constructs here.

**StackComposition**:
The ordered set of platform and application stacks created in `Program.cs`.
_Avoid_: scattering stack creation across construct library files.

## Boundaries

- Owns CDK app startup and stack instantiation order only.
- Uses constructs from `Mikepattyn.CDK.Constructs`; it should not define reusable constructs.
- Does not own API handler behavior; application backends own request semantics.

## Example dialogue

> **Newcomer:** Where do I add a new Fish environment stack?
>
> **Expert:** In `Mikepattyn.CDK/Program.cs`, following the existing Fish Data → Api → Edge loop.
