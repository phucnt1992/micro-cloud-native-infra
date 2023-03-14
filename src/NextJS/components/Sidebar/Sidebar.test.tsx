import { render, screen } from '@testing-library/react'
import Sidebar from './Sidebar'

describe('Sidebar', () => {
  it('renders a heading', () => {
    render(<Sidebar />)

    const heading = screen.getByRole('navigation')

    expect(heading).toBeInTheDocument()
  })
});
