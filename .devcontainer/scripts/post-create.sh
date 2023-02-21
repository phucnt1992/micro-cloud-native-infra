# Add the current directory to the safe list
git config --global --add safe.directory /workspaces/$(basename $PWD)

# Update the plugins list in .zshrc
sed -i 's/\(plugins=\).*/\1(git dotnet)/' ~/.zshrc
