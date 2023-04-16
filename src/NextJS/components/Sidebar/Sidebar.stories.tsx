import { Meta, StoryFn } from '@storybook/react';
import { HomeIcon, ListBulletIcon } from "@heroicons/react/24/solid";

import Sidebar from './Sidebar';
import { within, userEvent } from '@storybook/testing-library';
const defaultClassName = 'h-6 w-6 text-gray-500';
export default {
  title: 'Components/Sidebar',
  component: Sidebar,

} as Meta<typeof Sidebar>;

const Template: StoryFn<typeof Sidebar> = (_) => <Sidebar navItems={[
  {
    title: 'Dashboard',
    icon: <HomeIcon className={defaultClassName} />,
    href: '#'
  },
  {
    title: 'Todo List',
    icon: <ListBulletIcon className={defaultClassName} />,
    href: '#'
  }
]} />;

export const Default = Template.bind({});

Default.play = async ({ canvasElement }) => {
  const canvas = within(canvasElement);

  // 👇 Assert DOM structure
  await expect(
    canvas.getByTestId(
      'logo-sidebar'
    )
  ).toBeInTheDocument();
}

