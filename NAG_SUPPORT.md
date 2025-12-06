# PGN NAG (Numeric Annotation Glyph) Support

## What are NAGs?

NAGs are numeric codes used in PGN files to annotate moves with standard symbols. For example, `$6` after a move means it's a "dubious move" and should be displayed as `?!`.

## Supported NAG Translations

### Move Quality
| Code | Symbol | Meaning |
|------|--------|---------|
| $1 | ! | Good move |
| $2 | ? | Mistake |
| $3 | !! | Brilliant move |
| $4 | ?? | Blunder |
| $5 | !? | Interesting move |
| $6 | ?! | Dubious move |
| $7 | ☐ | Forced move (only move) |

### Position Evaluation
| Code | Symbol | Meaning |
|------|--------|---------|
| $10 | = | Equal position |
| $11 | = | Equal chances, quiet position |
| $13 | ∞ | Unclear position |
| $14 | ⩲ | White has slight advantage |
| $15 | ⩱ | Black has slight advantage |
| $16 | ± | White has moderate advantage |
| $17 | ∓ | Black has moderate advantage |
| $18 | +- | White has decisive advantage |
| $19 | -+ | Black has decisive advantage |

### Strategic Annotations
| Code | Symbol | Meaning |
|------|--------|---------|
| $22 | ⨀ | Zugzwang |
| $32 | ⟳ | Development advantage |
| $36 | ↑ | Initiative |
| $40 | → | Attack |
| $44 | ⇆ | Compensation for the material |
| $132 | ⟲ | Counterplay |
| $138 | ⊕ | Time trouble |
| $140 | ∆ | With the idea |
| $146 | N | Novelty |

## Example Usage

### In PGN File:
```
4... Bc5 $6 5. Bxf7+ Ke7
```

### Displayed in App:
```
4.   Bc5?! 
5.   Bxf7+
```

The `$6` is automatically translated to `?!` and displayed right after the move.

## How It Works

1. **PGN Parser** extracts NAGs from the PGN file into the `MoveNode.Nags` list
2. **VariationExtractor** translates NAG codes to symbols using `TranslateNAG()`
3. **Move display** shows the move with translated NAGs: `Bc5?!`

## Multiple NAGs

A move can have multiple NAGs, and they'll all be translated and concatenated:
```
Bc5 $6 $36  →  Bc5?!↑  (dubious move with initiative)
```

## Unknown NAGs

If a NAG code is not in our translation table, it will be displayed as-is (e.g., `$123`). This ensures nothing is lost from the original PGN.

## Standard Reference

These translations follow the PGN standard as defined in the [PGN Specification](https://ia902908.us.archive.org/26/items/pgn-standard-1994-03-12/PGN_standard_1994-03-12.txt).

## Future Enhancements

Additional NAG codes can be added to the `TranslateNAG()` function in `VariationExtractor.cs` if needed. The full PGN standard defines NAGs from $0 to $255.
