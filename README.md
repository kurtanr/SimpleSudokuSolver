# SimpleSudokuSolver

[![Build](https://img.shields.io/appveyor/ci/kurtanr/SimpleSudokuSolver.svg)](https://ci.appveyor.com/project/kurtanr/SimpleSudokuSolver)
[![Nuget](https://img.shields.io/nuget/v/SimpleSudokuSolver.svg?color=green)](https://www.nuget.org/packages/SimpleSudokuSolver/)
[![License](https://img.shields.io/github/license/kurtanr/SimpleSudokuSolver.svg)](https://github.com/kurtanr/SimpleSudokuSolver/blob/master/LICENSE)

SimpleSudokuSolver is a library for solving sudoku puzzles written in C#.

It supports solving of sudoku puzzles steps-by-step, using various solving strategies, from the simplest ones to the more complex ones. It can be used as a learning tool on how to solve sudoku puzzles.

Repository contains tests for the library and a UI which uses the library and can be used to play sudoku.

<p align="center">
![SimpleSudokuSolver.UI](https://raw.githubusercontent.com/kurtanr/SimpleSudokuSolver/master/images/SimpleSudokuSolver.UI.png)
</p>

Out of the well known [Sudoku Solving Techniques](https://sudoku9x9.com/sudoku_solving_techniques_9x9.html), the solver currently implements:
* Hidden Single
* Naked Single
* Locked Candidates
* Naked Pair
