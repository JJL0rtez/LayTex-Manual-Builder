echo off
echo # -------------------------------------
echo # -------------------------------------
echo #        Stonetown-Karate-Manual       
echo # -------------------------------------
echo # -------------------------------------
call git status
call git pull --all
echo.
echo # --------------------------------------
echo #                 Done
call git status
echo # --------------------------------------
echo.
@pause