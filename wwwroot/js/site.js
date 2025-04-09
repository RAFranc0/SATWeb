// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function() {
    const dropdownItems = document.querySelectorAll('.dropdown-submenu > a.dropdown-toggle');

    dropdownItems.forEach(item => {
        item.addEventListener('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            
            const allSubmenus = document.querySelectorAll('.dropdown-submenu > ul.dropdown-menu');
            allSubmenus.forEach(submenu => {
                if (submenu !== this.nextElementSibling) {
                    submenu.style.display = 'none';
                }
            });

            const currentSubmenu = this.nextElementSibling;
            currentSubmenu.style.display = currentSubmenu.style.display === 'block' ? 'none' : 'block';
        });
    });
    
    document.addEventListener('click', function(e) {
        if (!e.target.closest('.dropdown-submenu')) {
            const allSubmenus = document.querySelectorAll('.dropdown-submenu > ul.dropdown-menu');
            allSubmenus.forEach(submenu => {
                submenu.style.display = 'none';
            });
        }
    });
});

