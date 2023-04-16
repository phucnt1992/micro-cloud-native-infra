'use client';
import { kebabCase } from "@/lib/string-utils";

interface INavItem {
  title: string;
  icon: JSX.Element;
  href: string;
}

export interface ISidebarProps {
  navItems: INavItem[];
}

export default function Sidebar(props: ISidebarProps) {
  return (
    <aside id="logo-sidebar" className="fixed top-0 left-0 z-40 w-64 h-screen pt-20 transition-transform -translate-x-full bg-white border-r border-gray-200 sm:translate-x-0 dark:bg-gray-800 dark:border-gray-700" aria-label="Sidebar">
      <div className="h-full px-3 pb-4 overflow-y-auto bg-white dark:bg-gray-800">
        <ul className="space-y-2 font-medium">
          {
            props.navItems.map((item) => (
              <li key={kebabCase(item.title)}>
                <a href={item.href} className="flex items-center p-2 text-gray-900 rounded-lg dark:text-white hover:bg-gray-100 dark:hover:bg-gray-700">
                  {item.icon}
                  <span className="ml-3">{item.title}</span>
                </a>
              </li>
            ))
          }
        </ul>
      </div>
    </aside>
  );
}
