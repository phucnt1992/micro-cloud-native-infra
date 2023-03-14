import './globals.css'
import { FluentUIRegistry as StyleRegistry } from '@/lib/registry'

export const metadata = {
  title: 'Create Next App',
  description: 'Generated by create next app',
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <html lang="en">
      <body>
        <StyleRegistry>
          {children}
        </StyleRegistry>
      </body>
    </html>
  )
}