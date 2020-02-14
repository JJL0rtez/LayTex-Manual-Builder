echo off
echo # -------------------------------------
echo # -------------------------------------
echo #        Stonetown-Karate-Manual       
echo # -------------------------------------
echo # -------------------------------------
echo.
call git status
call git add .
call git status
call git commit -m "Update from Save Data batch"
call git status
call git push --all
echo.
echo # ------------------------------------
echo #                 Done
call git status                
echo # ------------------------------------
echo.
@pause