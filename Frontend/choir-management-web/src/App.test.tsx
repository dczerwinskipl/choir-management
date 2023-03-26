import React from "react";
import { render, screen } from "@testing-library/react";
import App from "./App";

test("renders learn react link", () => {
  render(<App />);
  const linkElement = screen.queryByText(/learn react/i);
  expect(linkElement).toBeInTheDocument();
});

test("test something", (done) => {
  render(<App />);
  done();
});

test("test something else", (done) => {
  render(<App />);
  done("exception");
});
