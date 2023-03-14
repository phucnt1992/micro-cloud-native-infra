import { ComponentMeta, ComponentStory } from '@storybook/react';

import Sidebar from './Sidebar';

export default {
  title: 'Components/Sidebar',
  component: Sidebar,
} as ComponentMeta<typeof Sidebar>;

const Template: ComponentStory<typeof Sidebar> = (args) => <Sidebar />;

export const Default = Template.bind({});
